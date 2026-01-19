using MediatR;

namespace InTicket.Application.Feauters.Profile.Queries.GetMyDelegation;

public class GetMyDelegationsRequest : IRequest <GetMyDelegationsResponse>
{
   public string currentUserId { get; set; }
}