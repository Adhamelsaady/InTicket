using FluentAssertions;
using InTicket.Application.Contracts;
using InTicket.Application.Feauters.Authentication.Confirmations.EmailConfirmations;
using InTicket.Domain;
using InTicket.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace InTicket.Tests.Features.Authentication;

public class EmailConfirmationRequestHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IOtpService> _mockOtpService;
    private readonly EmailConfirmationRequestHandler _handler;

    public EmailConfirmationRequestHandlerTests()
    {
        _mockUserManager = MockHelpers.MockUserManager();
        _mockOtpService = new Mock<IOtpService>();
        _handler = new EmailConfirmationRequestHandler(
            _mockUserManager.Object,
            _mockOtpService.Object);
    }
    private EmailConfirmationRequest ValidRequest(ApplicationUser user) => new()
    {
        Email = user.Email!,
        Otp = "123456"
    };

    private ApplicationUser UnconfirmedUserWithOtp(string email = "user@example.com") =>
        new ApplicationUser
        {
            Email = email,
            FirstName = "Bob",
            LastName = "Builder",
            EmailConfirmed = false,
            EmailConfirmationOtp = "123456",
            EmailConfirmationOtpExpiration = DateTime.UtcNow.AddMinutes(15),
            OtpAttempts = 0
        };

    [Fact]
    public async Task Handle_WithValidOtp_ReturnsTrueAndConfirmsEmail()
    {
        // Arrange
        var user = UnconfirmedUserWithOtp();
        var request = ValidRequest(user);
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(request.Otp, user.EmailConfirmationOtp, user.EmailConfirmationOtpExpiration))
            .Returns(true);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert        
        Assert.True(result);
        Assert.True(user.EmailConfirmed);
    }

    [Fact]
    public async Task Handle_WithValidOtp_ClearsOtpFieldsAfterConfirmation()
    {
        // Arrange
        var user = UnconfirmedUserWithOtp();
        var request = ValidRequest(user);
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .Returns(true);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(user.EmailConfirmationOtp);
        Assert.Null(user.EmailConfirmationOtpExpiration);
        Assert.Equal(0, user.OtpAttempts);
        Assert.Null(user.LastOtpAttemptAt);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        var request = new EmailConfirmationRequest { Email = "ghost@example.com", Otp = "000000" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_WithInvalidOtp_ReturnsFalse()
    {
        // Arrange
        var user = UnconfirmedUserWithOtp();
        var request = new EmailConfirmationRequest { Email = user.Email!, Otp = "WRONG1" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp("WRONG1", user.EmailConfirmationOtp, user.EmailConfirmationOtpExpiration))
            .Returns(false);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_WithInvalidOtp_IncrementsAttemptCounter()
    {
        // Arrange
        var user = UnconfirmedUserWithOtp();
        user.OtpAttempts = 2;
        var request = ValidRequest(user);
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .Returns(false);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, user.OtpAttempts);
    }

    [Fact]
    public async Task Handle_WhenAtMaxAttemptsAndWithinWindow_ReturnsFalseWithoutValidatingOtp()
    {
        // Arrange
        var user = UnconfirmedUserWithOtp();
        user.OtpAttempts = 10;
        user.LastOtpAttemptAt = DateTime.UtcNow.AddMinutes(-2); // still within 15-min window
        var request = ValidRequest(user);
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockOtpService.Verify(
            x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenLockoutWindowExpired_ResetsCounterAndProceedsWithValidation()
    {
        // Arrange
        var user = UnconfirmedUserWithOtp();
        user.OtpAttempts = 10;
        user.LastOtpAttemptAt = DateTime.UtcNow.AddMinutes(-20);
        var request = ValidRequest(user);
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .Returns(true);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert 
        Assert.True(result);
        _mockOtpService.Verify(
            x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUpdateFails_ReturnsFalse()
    {
        // Arrange
        var user = UnconfirmedUserWithOtp();
        var request = ValidRequest(user);
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .Returns(true);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Save failed" }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}
