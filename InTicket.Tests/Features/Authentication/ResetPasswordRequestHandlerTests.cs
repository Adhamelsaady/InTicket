using FluentAssertions;
using InTicket.Application.Contracts;
using InTicket.Application.Feauters.Authentication.Commands.ResetPassword;
using InTicket.Domain;
using InTicket.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace InTicket.Tests.Features.Authentication;

public class ResetPasswordRequestHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IOtpService> _mockOtpService;
    private readonly ResetPasswordRequestHandler _handler;

    public ResetPasswordRequestHandlerTests()
    {
        _mockUserManager = MockHelpers.MockUserManager();
        _mockOtpService = new Mock<IOtpService>();

        _handler = new ResetPasswordRequestHandler(
            _mockUserManager.Object,
            _mockOtpService.Object);
    }


    private ResetPasswordRequest ValidRequest(string email = "user@example.com") => new()
    {
        Email = email,
        Otp = "123456",
        NewPassword = "NewPassword123!"
    };

    private ApplicationUser UserWithValidOtp(string email = "user@example.com") =>
        new ApplicationUser
        {
            Email = email,
            FirstName = "Test",
            LastName = "User",
            PasswordResetOtp = "123456",
            PasswordResetOtpExpiration = DateTime.UtcNow.AddMinutes(15),
            OtpAttempts = 0
        };

    [Fact]
    public async Task Handle_WithValidOtp_ReturnsTrueAndResetsPassword()
    {
        // Arrange
        var request = ValidRequest();
        var user = UserWithValidOtp();
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(request.Otp, user.PasswordResetOtp, user.PasswordResetOtpExpiration))
            .Returns(true);
        _mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("reset-token");
        _mockUserManager.Setup(x => x.ResetPasswordAsync(user, It.IsAny<string>(), request.NewPassword))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_WithValidOtp_ClearsOtpFieldsAfterReset()
    {
        // Arrange
        var request = ValidRequest();
        var user = UserWithValidOtp();
        ApplicationUser? captured = null;
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .Returns(true);
        _mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("tok");
        _mockUserManager.Setup(x => x.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager
            .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .Callback<ApplicationUser>(u => captured = u)
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(captured);
        Assert.Null(captured.PasswordResetOtp);
        Assert.Null(captured.PasswordResetOtpExpiration);
        Assert.Equal(0, captured.OtpAttempts);
        Assert.Null(captured.LastOtpAttemptAt);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        var request = ValidRequest("ghost@example.com");
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
        var request = ValidRequest();
        var user = UserWithValidOtp();
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(request.Otp, user.PasswordResetOtp, user.PasswordResetOtpExpiration))
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
        var request = ValidRequest();
        var user = UserWithValidOtp();
        user.OtpAttempts = 3;
        ApplicationUser? captured = null;
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .Returns(false);
        _mockUserManager
            .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .Callback<ApplicationUser>(u => captured = u)
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        captured!.OtpAttempts.Should().Be(4);
    }

    [Fact]
    public async Task Handle_WhenUserExceeds10Attempts_ReturnsFalseImmediately()
    {
        // Arrange
        var request = ValidRequest();
        var user = UserWithValidOtp();
        user.OtpAttempts = 10;
        user.LastOtpAttemptAt = DateTime.UtcNow.AddMinutes(-5);

        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _mockOtpService.Verify(
            x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIdentityResetFails_ReturnsFalse()
    {
        // Arrange
        var request = ValidRequest();
        var user = UserWithValidOtp();
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .Returns(true);
        _mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("tok");
        _mockUserManager.Setup(x => x.ResetPasswordAsync(user, "tok", request.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_AfterLockoutWindowExpires_ResetsAttemptCounterAndAllowsNewAttempt()
    {
        // Arrange
        var request = ValidRequest();
        var user = UserWithValidOtp();
        user.OtpAttempts = 10;
        user.LastOtpAttemptAt = DateTime.UtcNow.AddMinutes(-20);
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .Returns(false);
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockOtpService.Verify(
            x => x.ValidateOtp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()),
            Times.Once);
    }
}
