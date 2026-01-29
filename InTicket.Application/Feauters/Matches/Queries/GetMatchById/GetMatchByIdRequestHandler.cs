using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using MediatR;

namespace InTicket.Application.Feauters.Matchs.Queries;

public class GetMatchByIdRequestHandler : IRequestHandler<GetMatchByIdRequest , GetMatchResponse>
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;

    public GetMatchByIdRequestHandler(IMatchRepository matchRepository 
        , IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }
    
    public async Task<GetMatchResponse> Handle(GetMatchByIdRequest request, CancellationToken cancellationToken)
    {
        var matchEntity = await _matchRepository.GetMatchByIdAsync(request.Id , request.IsRequestedByAdmin);
        if (matchEntity == null || !matchEntity.IsActive)
        {
            return null;
        }
        var matchToReturn = _mapper.Map<GetMatchResponse>(matchEntity);
        var homeTeam = _mapper.Map<TeamDto>(matchEntity.HomeTeam);
        var awayTeam = _mapper.Map<TeamDto>(matchEntity.AwayTeam);
        matchToReturn.HomeTeam = homeTeam;
        matchToReturn.AwayTeam = awayTeam;
        return matchToReturn;
    }
}