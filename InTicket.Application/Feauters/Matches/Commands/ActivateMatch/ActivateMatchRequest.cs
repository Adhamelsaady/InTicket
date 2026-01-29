using MediatR;

namespace InTicket.Application.Feauters.Matches.Commands.ActivateMatch;

public class ActivateMatchRequest : IRequest<bool>
{
    public Guid Id { get; set; }
}