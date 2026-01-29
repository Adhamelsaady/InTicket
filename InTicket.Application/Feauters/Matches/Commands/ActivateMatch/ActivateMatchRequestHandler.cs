using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Matches.Commands.ActivateMatch;
using MediatR;

public class ActivateMatchRequestHandler : IRequestHandler<ActivateMatchRequest , bool>
{
    
    private readonly IBaseRepository <InTicket.Domain.Match> _matchRepository;

    public ActivateMatchRequestHandler(IBaseRepository <InTicket.Domain.Match> matchRepository)
    {
        _matchRepository = matchRepository;
    }
    
    public async Task<bool> Handle(ActivateMatchRequest request, CancellationToken cancellationToken)
    {
        var match = await _matchRepository.GetByIdAsync(request.Id);
        if (match == null) return false;
        if(!match.IsActive)
        {
            match.IsActive = true;
            match.FanPriorityBookingStart = DateTime.Now.AddMinutes(1);
        }
        
        _matchRepository.SaveChangesAsync();
        return true;
    }
}