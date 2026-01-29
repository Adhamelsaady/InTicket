using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using MediatR;

namespace InTicket.Application.Feauters.Matchs.Queries.GetAllMatches;

public class GetAllMatchesRequestHandler : IRequestHandler<GetAllMatchesRequest, PagedResult<GetMatchResponse>>
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;

    public GetAllMatchesRequestHandler(IMatchRepository matchRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetMatchResponse>> Handle(GetAllMatchesRequest request, CancellationToken cancellationToken)
    {
        var matches = await _matchRepository.GetAllMatchesAsync(request.MatchResourceParameters , request.IsRequestedByAdmin);
        var matchesToReturn = _mapper.Map<List<GetMatchResponse>>(matches.Items);
        var result = new PagedResult<GetMatchResponse>();
        result.TotalCount = matches.TotalCount;
        result.PageNumber = matches.PageNumber;
        result.PageSize = matches.PageSize;
        result.Items = matchesToReturn;
        return result;
    }
}