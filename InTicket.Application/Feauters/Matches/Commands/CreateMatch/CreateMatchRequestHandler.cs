using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Matches.Commands.CreateMatch;

public class CreateMatchRequestHandler : IRequestHandler<CreateMatchRequest , CreateMatchRequestResponse>
{
    private readonly IBaseRepository <Match>  _matchRepository;
    private readonly IMapper _mapper;
    public CreateMatchRequestHandler(IBaseRepository<Match> matchRepository
    , IMapper mapper)
    {
        _mapper = mapper;
        _matchRepository = matchRepository;
    }
    
    public async Task<CreateMatchRequestResponse> Handle(CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var matchEntity = _mapper.Map<Match>(request);
        await _matchRepository.AddAsync(matchEntity);
        await _matchRepository.SaveChangesAsync();
        return new CreateMatchRequestResponse() { Id = matchEntity.Id };
    }
}