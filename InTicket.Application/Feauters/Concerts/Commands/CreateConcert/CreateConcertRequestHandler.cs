using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Concerts.Commands.CreateConcert;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.Commands;

public class CreateConcertRequestHandler : IRequestHandler<CreateConcertRequest, CreateConcertRequestResponse>
{
    private readonly IBaseRepository<Concert> _concertsRepository;
    private readonly IBaseRepository<ConcertTicket> _concertTicketsRepository;
    private readonly IMapper _mapper;

    public CreateConcertRequestHandler(IBaseRepository<Concert> concertsRepository,
        IBaseRepository<ConcertTicket> ticketsRepository, IMapper mapper)
    {
        _concertsRepository = concertsRepository;
        _concertTicketsRepository = ticketsRepository;
        _mapper = mapper;
    }

    public async Task<CreateConcertRequestResponse> Handle(CreateConcertRequest request,
        CancellationToken cancellationToken)
    {
        var concertToAdd = _mapper.Map<Concert>(request);
        await _concertsRepository.AddAsync(concertToAdd);
        await _concertsRepository.SaveChangesAsync();

        foreach (var item in request.TicketsDistribution)
        {
            var @class = item.Key;
            var ticketDate = item.Value;
            for (int i = 0; i < ticketDate.Count; ++i)
            {
                var tikcetToAdd = new ConcertTicket()
                {
                    TicketId = Guid.NewGuid(),
                    Price = ticketDate.Price,
                    TicketClass = @class,
                    ConcertId = concertToAdd.Id
                };
            }
        }

        await _concertTicketsRepository.SaveChangesAsync();
        return new CreateConcertRequestResponse() { Id = concertToAdd.Id };
    }
}