using MediatR;

namespace InTicket.Application.Feauters.Profile.Commands.DeleteDelegation;

public class DeleteDelegationRequest : IRequest<bool>
{
    public Guid DelegationId { get; set; }
    public string DelegatorId  { get; set; }
}