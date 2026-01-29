using MediatR;

namespace InTicket.Application.Feauters.Concerts.ActivateConcert;

public class ActivateConcertRequest : IRequest <bool>
{
    public Guid Id { get; set; }
}