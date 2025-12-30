using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.Commands.DeleteConcert;

public class DeleteConcertRequestHandler : IRequestHandler<DeleteConcertRequest , bool>
{
    private readonly IBaseRepository<Concert> _concertRepository;
    
    public async Task<bool> Handle(DeleteConcertRequest request, CancellationToken cancellationToken)
    {
        var concertEntity = await _concertRepository.GetByIdAsync(request.Id);
        if (concertEntity == null)
        {
            return false;
        }
        await _concertRepository.DeleteAsync(concertEntity);
        await _concertRepository.SaveChangesAsync();
        return true;
    }
}