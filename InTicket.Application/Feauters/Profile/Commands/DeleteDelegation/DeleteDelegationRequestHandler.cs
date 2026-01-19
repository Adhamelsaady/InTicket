using System.Runtime.InteropServices.JavaScript;
using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Commands.DeleteDelegation;

public class DeleteDelegationRequestHandler : IRequestHandler<DeleteDelegationRequest , bool>
{
    private readonly IBaseRepository<Delegation>  _delegationRepository;

    public DeleteDelegationRequestHandler(IBaseRepository<Delegation> delegationRepository)
    {
        _delegationRepository = delegationRepository;
    }
    public async Task<bool> Handle(DeleteDelegationRequest request, CancellationToken cancellationToken)
    {
        var delegation = await _delegationRepository.GetByIdAsync(request.DelegationId);
        if (delegation.DelegatorId != request.DelegatorId)
        {
            return  false;
        }
        await _delegationRepository.DeleteAsync(delegation);
        await _delegationRepository.SaveChangesAsync();
        return true;
    }
}