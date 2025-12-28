using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IMatchRepository
{
    Task<PagedResult<Match>> GetAllMatchesAsync(MatchResourceParameters matchResourceParameters);
    Task <Match> GetMatchByIdAsync(Guid matchId);
}