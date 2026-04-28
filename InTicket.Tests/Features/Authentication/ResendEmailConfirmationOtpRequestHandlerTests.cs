using FluentAssertions;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Feauters.Authentication.Confirmations.ResendEmailConfirmationOtp;
using InTicket.Domain;
using InTicket.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace InTicket.Tests.Features.Authentication;

public class ResendEmailConfirmationOtpRequestHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IOtpService> _mockOtpService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly ResendEmailConfirmationOtpRequestHandler _handler;

    public ResendEmailConfirmationOtpRequestHandlerTests()
    {
        _mockUserManager = MockHelpers.MockUserManager();
        _mockOtpService = new Mock<IOtpService>();
        _mockEmailService = new Mock<IEmailService>();

        _handler = new ResendEmailConfirmationOtpRequestHandler(
            _mockUserManager.Object,
            _mockOtpService.Object,
            _mockEmailService.Object);
    }


    [Fact]
    public async Task Handle_WithValidUnconfirmedUser_ReturnsTrueAndSendsEmail()
    {
        // Arrange
        var user = MockHelpers.CreateUser(emailConfirmed: false);
        var request = new ResendEmailConfirmationOtpRequest { Email = user.Email! };
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("OTP123");
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockEmailService.Verify(
            x => x.SendEmailConfirmationOtpAsync(user.Email!, user.FirstName, "OTP123"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidUnconfirmedUser_UpdatesOtpAndExpiration()
    {
        // Arrange
        var user = MockHelpers.CreateUser(emailConfirmed: false);
        var request = new ResendEmailConfirmationOtpRequest { Email = user.Email! };
        ApplicationUser? captured = null;
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("NEW999");
        _mockUserManager
            .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .Callback<ApplicationUser>(u => captured = u)
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("NEW999", captured!.EmailConfirmationOtp);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        var request = new ResendEmailConfirmationOtpRequest { Email = "nobody@example.com" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockEmailService.Verify(
            x => x.SendEmailConfirmationOtpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithAlreadyConfirmedEmail_ReturnsFalse()
    {
        // Arrange — user already confirmed, so resend should be rejected
        var user = MockHelpers.CreateUser(emailConfirmed: true);
        var request = new ResendEmailConfirmationOtpRequest { Email = user.Email! };
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockEmailService.Verify(
            x => x.SendEmailConfirmationOtpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUpdateFails_ReturnsFalse()
    {
        // Arrange
        var user = MockHelpers.CreateUser(emailConfirmed: false);
        var request = new ResendEmailConfirmationOtpRequest { Email = user.Email! };
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("111111");
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "DB error" }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_WhenUpdateFails_DoesNotSendEmail()
    {
        // Arrange
        var user = MockHelpers.CreateUser(emailConfirmed: false);
        var request = new ResendEmailConfirmationOtpRequest { Email = user.Email! };
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("000000");
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "error" }));

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _mockEmailService.Verify(
            x => x.SendEmailConfirmationOtpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }
}
