using FluentAssertions;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Feauters.Authentication.Commands.ForgotPassword;
using InTicket.Domain;
using InTicket.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace InTicket.Tests.Features.Authentication;

public class ForgotPasswordRequestHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IOtpService> _mockOtpService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly ForgotPasswordRequestHandler _handler;

    public ForgotPasswordRequestHandlerTests()
    {
        _mockUserManager  = MockHelpers.MockUserManager();
        _mockOtpService   = new Mock<IOtpService>();
        _mockEmailService = new Mock<IEmailService>();

        _handler = new ForgotPasswordRequestHandler(
            _mockUserManager.Object,
            _mockOtpService.Object,
            _mockEmailService.Object);
    }

    [Fact]
    public async Task Handle_WithExistingConfirmedUser_ReturnsTrueAndSendsOtp()
    {
        // Arrange
        var user    = MockHelpers.CreateUser(emailConfirmed: true);
        var request = new ForgotPasswordRequest { Email = user.Email! };
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("ABCDEF");
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert        
        Assert.True(result);
        _mockEmailService.Verify(
            x => x.SendPasswordResetOtpAsync(user.Email!, user.FirstName, "ABCDEF"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingConfirmedUser_StoresOtpOnUser()
    {
        // Arrange
        var user = MockHelpers.CreateUser(emailConfirmed: true);
        var request = new ForgotPasswordRequest { Email = user.Email! };
        ApplicationUser? captured = null;
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("ZZZZZZ");
        _mockUserManager
            .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .Callback<ApplicationUser>(u => captured = u)
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(captured);
        Assert.Equal("ZZZZZZ", captured.PasswordResetOtp);
    }

    [Fact]
    public async Task Handle_WithNonExistentEmail_ReturnsTrueWithoutSendingEmail()
    {
        // Arrange
        var request = new ForgotPasswordRequest { Email = "nobody@example.com" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockEmailService.Verify(
            x => x.SendPasswordResetOtpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithUnconfirmedEmail_ReturnsTrueWithoutSendingEmail()
    {
        // Arrange
        var user = MockHelpers.CreateUser(emailConfirmed: false);
        var request = new ForgotPasswordRequest { Email = user.Email! };
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockEmailService.Verify(
            x => x.SendPasswordResetOtpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUpdateFails_ThrowsException()
    {
        // Arrange
        var user = MockHelpers.CreateUser(emailConfirmed: true);
        var request = new ForgotPasswordRequest { Email = user.Email! };
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("123456");
        _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "DB error" }));

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<Exception>(act);
    }
}
