using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Matches.Commands.DeleteMatch;

public class DeleteMatchRequestHandler : IRequestHandler<DeleteMatchRequest , bool>
{
    private readonly IBaseRepository <Match> _matchRepository;
    
    public async Task<bool> Handle(DeleteMatchRequest request, CancellationToken cancellationToken)
    {
        var matchToDelete = await _matchRepository.GetByIdAsync(request.Id);
        if (matchToDelete == null)
        {
            return false;
        }
        await _matchRepository.DeleteAsync(matchToDelete);
        await _matchRepository.SaveChangesAsync();
        return true;
    }
}