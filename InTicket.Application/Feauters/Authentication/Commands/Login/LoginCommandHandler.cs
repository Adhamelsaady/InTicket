using InTicket.Application.Contracts;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InTicket.Application.Feauters.Authentication.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGeneration _jwtTokenGeneration;

    public LoginCommandHandler(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGeneration jwtTokenGeneration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGeneration = jwtTokenGeneration;
    }

    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("No user found");
        }

        if (!user.EmailConfirmed)
        {
            throw new Exception("Email not confirmed");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new Exception("Incorrect email or password");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtTokenGeneration.GenerateJwtToken(user, roles.ToList());

        return new AuthenticationResponse
        {
            Token = token,
            Email = user.Email!,
            FullName = user.FirstName + " " + user.LastName,
            Roles = roles.ToList()
        };
    }
}