using MediatR;
namespace InTicket.Application.Feauters.Matches.Commands.DeleteMatch;

public class DeleteMatchRequest : IRequest <bool>
{
    public Guid Id { get; set; }
}