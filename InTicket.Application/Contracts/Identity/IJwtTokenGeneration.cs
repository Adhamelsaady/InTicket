using InTicket.Domain;

namespace InTicket.Application.Contracts;

public interface IJwtTokenGeneration
{
    public string GenerateJwtToken(ApplicationUser user, List<string> roles);
}