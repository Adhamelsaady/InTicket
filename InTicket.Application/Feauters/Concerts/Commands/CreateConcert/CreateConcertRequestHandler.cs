using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Concerts.Commands.CreateConcert;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.Commands;

public class CreateConcertRequestHandler : IRequestHandler<CreateConcertRequest , CreateConcertRequestResponse>
{
    private readonly IBaseRepository<Concert> _concertsRepository;
    private readonly IMapper _mapper;

    public CreateConcertRequestHandler(IBaseRepository<Concert> concertsRepository, IMapper mapper)
    {
        _concertsRepository = concertsRepository;
        _mapper = mapper;
    }
    
    public async Task<CreateConcertRequestResponse> Handle(CreateConcertRequest request, CancellationToken cancellationToken)
    {
        var concertToAdd = _mapper.Map<Concert>(request);
        await _concertsRepository.AddAsync(concertToAdd);
        await _concertsRepository.SaveChangesAsync();
        return new CreateConcertRequestResponse() { Id = concertToAdd.Id };
    }
}