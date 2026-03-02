using InTicket.Domain;
using InTicket.Domain.Dtos;

namespace InTicket.Application.Contracts;

public interface IJwtTokenGeneration
{
    public Task <TokenResult> GenerateJwtToken(ApplicationUser user, List<string> roles);
}