using AutoMapper;
using FluentAssertions;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Domain;
using InTicket.Domain.Dtos;
using InTicket.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace InTicket.Tests.Features.Authentication;

public class RegisterCommandHandlerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IOtpService> _mockOtpService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IJwtTokenGeneration> _mockJwtTokenGeneration;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _mockUserManager = MockHelpers.MockUserManager();
        _mockOtpService = new Mock<IOtpService>();
        _mockMapper = new Mock<IMapper>();
        _mockEmailService = new Mock<IEmailService>();
        _mockJwtTokenGeneration = new Mock<IJwtTokenGeneration>();

        _handler = new RegisterCommandHandler(
            _mockUserManager.Object,
            _mockOtpService.Object,
            _mockMapper.Object,
            _mockEmailService.Object,
            _mockJwtTokenGeneration.Object);
    }

    private RegisterCommand ValidCommand() => new()
    {
        Email = "newuser@example.com",
        Password = "Password123!",
        FirstName = "Jane",
        LastName = "Doe",
        UserName = "janedoe",
        PhoneNumber = "01012345678",
        NationalId = "12345678901234",
        FavoriteTeamId = Guid.NewGuid()
    };

    private void SetupNoExistingUsers()
    {
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null!);
        _mockUserManager.Setup(x => x.Users)
            .Returns(new List<ApplicationUser>().AsQueryable());
    }

    private void SetupSuccessfulCreate(ApplicationUser user)
    {
        _mockMapper.Setup(x => x.Map<ApplicationUser>(It.IsAny<RegisterCommand>()))
            .Returns(user);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"))
            .ReturnsAsync(IdentityResult.Success);
        _mockJwtTokenGeneration
            .Setup(x => x.GenerateJwtToken(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()))
            .ReturnsAsync(new TokenResult { Token = "access-token", RefreshToken = "refresh-token" });
    }


    [Fact]
    public async Task Handle_WithValidNewUser_ReturnsSuccess()
    {
        // Arrange
        var command = ValidCommand();
        var user = MockHelpers.CreateUser(command.Email, command.FirstName, command.LastName);
        SetupNoExistingUsers();
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("654321");
        SetupSuccessfulCreate(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(command.Email, result.Email);
        Assert.Contains("User", result.Roles);
    }

    [Fact]
    public async Task Handle_WithValidNewUser_SendsConfirmationEmail()
    {
        // Arrange
        var command = ValidCommand();
        var user = MockHelpers.CreateUser(command.Email, command.FirstName, command.LastName);
        SetupNoExistingUsers();
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("111111");
        SetupSuccessfulCreate(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockEmailService.Verify(
            x => x.SendEmailConfirmationOtpAsync(
                It.Is<string>(e => e == command.Email),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ReturnsFailureWithError()
    {
        // Arrange
        var command = ValidCommand();
        var existingUser = MockHelpers.CreateUser(command.Email, "Existing", "User");
        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(result.Errors, e => e.Contains("already registered"));
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_NeverCallsCreateAsync()
    {
        // Arrange
        var command = ValidCommand();
        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(MockHelpers.CreateUser(command.Email));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockUserManager.Verify(
            x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithDuplicateNationalId_ReturnsFailure()
    {
        // Arrange
        var command = ValidCommand();
        // No user found by email…
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null!);
        // … but an existing user has the same NationalId
        var existingByNid = MockHelpers.CreateUser("other@example.com");
        existingByNid.NationalId = command.NationalId;
        _mockUserManager.Setup(x => x.Users)
            .Returns(new List<ApplicationUser> { existingByNid }.AsQueryable());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public async Task Handle_WhenIdentityCreateFails_ReturnsFailureWithGenericError()
    {
        // Arrange
        var command = ValidCommand();
        var user = MockHelpers.CreateUser(command.Email, command.FirstName, command.LastName);
        SetupNoExistingUsers();
        _mockMapper.Setup(x => x.Map<ApplicationUser>(It.IsAny<RegisterCommand>())).Returns(user);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Passwords do not match." }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Something went wrong. Please try again.", result.Errors.First());
    }

    [Fact]
    public async Task Handle_WhenIdentityCreateFails_NeverSendsEmail()
    {
        // Arrange
        var command = ValidCommand();
        var user = MockHelpers.CreateUser(command.Email, command.FirstName, command.LastName);
        SetupNoExistingUsers();
        _mockMapper.Setup(x => x.Map<ApplicationUser>(It.IsAny<RegisterCommand>())).Returns(user);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockEmailService.Verify(
            x => x.SendEmailConfirmationOtpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSuccessful_AssignsOtpToUser()
    {
        // Arrange
        var command = ValidCommand();
        ApplicationUser? capturedUser = null;
        var user = MockHelpers.CreateUser(command.Email, command.FirstName, command.LastName);
        SetupNoExistingUsers();
        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("999999");
        _mockMapper.Setup(x => x.Map<ApplicationUser>(It.IsAny<RegisterCommand>())).Returns(user);
        _mockUserManager
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Callback<ApplicationUser, string>((u, _) => capturedUser = u)
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"))
            .ReturnsAsync(IdentityResult.Success);
        _mockJwtTokenGeneration
            .Setup(x => x.GenerateJwtToken(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()))
            .ReturnsAsync(new TokenResult { Token = "t", RefreshToken = "r" });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert – OTP was set before CreateAsync was called
        Assert.NotNull(capturedUser);
        Assert.Equal("999999", capturedUser.EmailConfirmationOtp);
        Assert.NotNull(capturedUser.EmailConfirmationOtpExpiration);
    }
}
