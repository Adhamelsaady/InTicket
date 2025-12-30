using MediatR;

namespace InTicket.Application.Feauters.Concerts.Commands.DeleteConcert;

public class DeleteConcertRequest : IRequest<bool>
{
    public Guid Id { get; set; }
}