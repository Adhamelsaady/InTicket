using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using InTicket.Application.ResourceParameters;
using MediatR;

namespace InTicket.Application.Feauters.Matchs.Queries.GetAllMatches;

public class GetAllMatchesRequest : IRequest<PagedResult<GetMatchResponse>>
{
    public MatchResourceParameters MatchResourceParameters { get; set; }
}