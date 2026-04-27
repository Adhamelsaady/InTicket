using FluentAssertions;
using InTicket.Application.Contracts;
using InTicket.Application.Feauters.Authentication.Login;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using InTicket.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace InTicket.Tests.Features.Authentication;

public class LoginCommandHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<IJwtTokenGeneration> _mockJwtTokenGeneration;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _mockUserManager    = MockHelpers.MockUserManager();
        _mockSignInManager  = MockHelpers.MockSignInManager(_mockUserManager);
        _mockJwtTokenGeneration = new Mock<IJwtTokenGeneration>();

        _handler = new LoginCommandHandler(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockJwtTokenGeneration.Object);
    }

    private LoginCommand ValidCommand(string email = "user@example.com", string password = "Password123!")
        => new() { Email = email, Password = password };

    private void SetupSuccessfulSignIn(ApplicationUser user, IList<string> roles)
    {
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockSignInManager
            .Setup(x => x.CheckPasswordSignInAsync(user, It.IsAny<string>(), false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);
        _mockJwtTokenGeneration
            .Setup(x => x.GenerateJwtToken(user, It.IsAny<List<string>>()))
            .ReturnsAsync(new TokenResult { Token = "access-token", RefreshToken = "refresh-token" });
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ReturnsSuccessWithToken()
    {
        // Arrange
        var user    = MockHelpers.CreateUser(emailConfirmed: true);
        var command = ValidCommand(user.Email!);
        SetupSuccessfulSignIn(user, new List<string> { "User" });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("access-token", result.Token);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ReturnsCorrectFullName()
    {
        // Arrange
        var user    = MockHelpers.CreateUser(firstName: "Alice", lastName: "Smith");
        var command = ValidCommand(user.Email!);
        SetupSuccessfulSignIn(user, new List<string> { "User" });

        // Act
        var result = (AuthenticationResponse)await _handler.Handle(command, CancellationToken.None);

        // Assert        
        Assert.Equal("Alice Smith", result.FullName);
    }

    [Fact]
    public async Task Handle_WithNonExistentEmail_ReturnsFailure()
    {
        // Arrange
        var command = ValidCommand("ghost@example.com");
        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("Wrong email or password"));
    }

    [Fact]
    public async Task Handle_WithUnconfirmedEmail_ReturnsFailure()
    {
        // Arrange
        var user    = MockHelpers.CreateUser(emailConfirmed: false);
        var command = ValidCommand(user.Email!);
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert        
        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("Wrong email or password"));
    }

    [Fact]
    public async Task Handle_WithWrongPassword_ReturnsFailure()
    {
        // Arrange
        var user    = MockHelpers.CreateUser(emailConfirmed: true);
        var command = ValidCommand(user.Email!, "WrongPass!");
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockSignInManager
            .Setup(x => x.CheckPasswordSignInAsync(user, "WrongPass!", false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert        
        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("Wrong email or password"));
    }

    [Fact]
    public async Task Handle_WithWrongPassword_NeverGeneratesToken()
    {
        // Arrange
        var user    = MockHelpers.CreateUser(emailConfirmed: true);
        var command = ValidCommand(user.Email!, "BadPass!");
        _mockUserManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _mockSignInManager
            .Setup(x => x.CheckPasswordSignInAsync(user, "BadPass!", false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockJwtTokenGeneration.Verify(
            x => x.GenerateJwtToken(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ReturnsUserRoles()
    {
        // Arrange
        var user    = MockHelpers.CreateUser(emailConfirmed: true);
        var command = ValidCommand(user.Email!);
        SetupSuccessfulSignIn(user, new List<string> { "User", "Admin" });

        // Act
        var result = (AuthenticationResponse)await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Contains(result.Roles, r => r == "User");
        Assert.Contains(result.Roles, r => r == "Admin");
    }
}
