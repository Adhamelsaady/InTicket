using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IDelegationsRepository
{
    Task<Delegation> GetDelegation(string delegatorId);
}