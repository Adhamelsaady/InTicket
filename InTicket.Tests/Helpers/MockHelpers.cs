using InTicket.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace InTicket.Tests.Helpers;

public static class MockHelpers
{
    public static Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object,
           new Mock<IOptions<IdentityOptions>>().Object,
           new Mock<IPasswordHasher<ApplicationUser>>().Object,
           Array.Empty<IUserValidator<ApplicationUser>>(),
           Array.Empty<IPasswordValidator<ApplicationUser>>(),
           new Mock<ILookupNormalizer>().Object,
           new Mock<IdentityErrorDescriber>().Object,
           new Mock<IServiceProvider>().Object,
           new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        userManagerMock.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
        userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

        return userManagerMock;
    }

    public static Mock<SignInManager<ApplicationUser>> MockSignInManager(
        Mock<UserManager<ApplicationUser>> userManagerMock)
    {
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var logger = new Mock<ILogger<SignInManager<ApplicationUser>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();
        var confirmation = new Mock<IUserConfirmation<ApplicationUser>>();
        return new Mock<SignInManager<ApplicationUser>>(
            userManagerMock.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            options.Object,
            logger.Object,
            schemes.Object,
            confirmation.Object);
    }

    public static ApplicationUser CreateUser(
        string email = "user@example.com",
        string firstName = "John",
        string lastName = "Doe",
        bool emailConfirmed = true,
        string nationalId = "12345678901234")
        => new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = emailConfirmed,
            NationalId = nationalId,
            InTicketId = Guid.NewGuid()
        };
}

