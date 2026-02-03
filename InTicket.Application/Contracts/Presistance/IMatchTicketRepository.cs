using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IMatchTicketRepository
{
    Task<bool> UserHasTicketForMatchAsync(string userId, Guid matchId);
    Task<MatchTicket> GetRandomTicketAsync(MatchTicketClass matchTicketClass, Guid matchId, string UserId);
}