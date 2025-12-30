using InTicket.Application.Feauters.Concerts.Queries.Common;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.Queries.GetConcertById;

public class GetConcertByIdRequest : IRequest<GetConcertResponse>
{
    public Guid Id { get; set; }
}