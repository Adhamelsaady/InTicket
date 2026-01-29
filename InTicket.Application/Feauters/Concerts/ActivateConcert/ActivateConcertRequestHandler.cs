using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.ActivateConcert;

public class ActivateConcertRequestHandler : IRequestHandler <ActivateConcertRequest,  bool> 
{
    private readonly IBaseRepository <Concert> _concertRepository;

    public ActivateConcertRequestHandler(IBaseRepository <Concert>  concertRepository)
    {
        _concertRepository = concertRepository;
    }


    public async Task<bool> Handle(ActivateConcertRequest request, CancellationToken cancellationToken)
    {
        var concert = await _concertRepository.GetByIdAsync(request.Id);
        if (concert == null) return false;
        if (!concert.IsActive)
        {
            concert.IsActive = true;
            concert.BookingStartDate = DateTime.Now.AddMinutes(10);
        }

        return true;
    }
}