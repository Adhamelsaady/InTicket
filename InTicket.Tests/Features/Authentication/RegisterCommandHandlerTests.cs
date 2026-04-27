using AutoMapper;
using FluentAssertions;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        _mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
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

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSuccess()
    {
        // Arrange
        var command = new RegisterCommand 
        { 
            Email = "test@example.com", 
            Password = "Password123!", 
            FirstName = "John", 
            LastName = "Doe",
            NationalId = "12345678901234"
        };
        
        var user = new ApplicationUser { Email = command.Email, FirstName = command.FirstName, LastName = command.LastName };
        
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null!);
            
        // Mocking Users IQueryable for NationalId check
        var users = new List<ApplicationUser>().AsQueryable();
        _mockUserManager.Setup(x => x.Users).Returns(users);

        _mockOtpService.Setup(x => x.GenerateOtp()).Returns("123456");
        _mockMapper.Setup(x => x.Map<ApplicationUser>(It.IsAny<RegisterCommand>())).Returns(user);
        
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
            
        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockJwtTokenGeneration.Setup(x => x.GenerateJwtToken(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new JwtTokenResponse { RefreshToken = "fake-refresh", AccessToken = "fake-access" });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Email.Should().Be(command.Email);
        _mockEmailService.Verify(x => x.SendEmailConfirmationOtpAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingEmail_ShouldReturnFailure()
    {
        // Arrange
        var command = new RegisterCommand { Email = "existing@example.com" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(new ApplicationUser { Email = command.Email, FirstName = "Existing", LastName = "User" });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.First().Should().Contain("already registered");
    }

    [Fact]
    public async Task Handle_IdentityFailure_ShouldReturnFailure()
    {
        // Arrange
        var command = new RegisterCommand { Email = "test@example.com", Password = "123" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);
        _mockUserManager.Setup(x => x.Users).Returns(new List<ApplicationUser>().AsQueryable());
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Weak password" }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.First().Should().Be("Something went wrong. Please try again.");
    }
}
