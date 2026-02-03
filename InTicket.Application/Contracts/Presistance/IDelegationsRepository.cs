using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IDelegationsRepository
{
    Task<Delegation> GetDelegationAsync(string delegatorId);
    Task <bool> HasDelegationAsync(string delegatorId);
    Task<Delegation> AddAsync(Delegation delegation);
    Task<bool> IsDelegatedAsync(string delegatorId, string @delegate);
}