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
        if (user == null || !user.EmailConfirmed)
        {
            return new AuthenticationResponse()
            {
                Success = false,
                Errors = new List<string>() {"Wrong email or password"}
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            return new AuthenticationResponse()
            {
                Success = false,
                Errors = new List<string>() {"Wrong email or password"}
            };
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = await _jwtTokenGeneration.GenerateJwtToken(user, roles.ToList());

        return new AuthenticationResponse
        {
            Success = true,
            Token = token.Token,
            RefreshToken = token.RefreshToken,
            Email = user.Email!,
            FullName = user.FirstName + " " + user.LastName,
            Roles = roles.ToList()
        };
    }
}