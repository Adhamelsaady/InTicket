using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Concerts.Queries.Common;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using InTicket.Application.ResourceParameters;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.Queries.GetAllConcerts;

public class GetAllConcertsRequest : IRequest <PagedResult<GetConcertResponse>>
{
    public ConcertResourceParameters ConcertResourceParameters { get; set; }
    public bool IsRequestedByAdmin { get; set; }
}