using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Matches.Commands.CreateMatch;

public class CreateMatchRequestHandler : IRequestHandler<CreateMatchRequest , CreateMatchRequestResponse>
{
    private readonly IBaseRepository <Match>  _matchRepository;
    private readonly IBaseRepository <MatchTicket>  _ticketRepository;
    private readonly IMapper _mapper;
    public CreateMatchRequestHandler(IBaseRepository<Match> matchRepository
    , IBaseRepository<MatchTicket> ticketRepository, IMapper mapper)
    {
        _mapper = mapper;
        _ticketRepository = ticketRepository;
        _matchRepository = matchRepository;
    }
    
    public async Task<CreateMatchRequestResponse> Handle(CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var matchEntity = _mapper.Map<Match>(request);
        
        await _matchRepository.AddAsync(matchEntity);
        await _matchRepository.SaveChangesAsync();
        foreach (var item in request.TicketsDistribution)
        {
            var ticketData = item.Value;
            for(int i = 0; i < ticketData.Count; ++i)
            {
                var ticketToAdd = new MatchTicket()
                {
                    TicketId = Guid.NewGuid(),
                    Price = ticketData.Price,
                    MatchId = matchEntity.Id,
                    HomeTeamTicket = ticketData.IsHomeTeam,
                    TicketClass = item.Key,
                };
                await _ticketRepository.AddAsync(ticketToAdd);
            }
        }
        await _ticketRepository.SaveChangesAsync();
        return new CreateMatchRequestResponse() { Id = matchEntity.Id };
    }
}