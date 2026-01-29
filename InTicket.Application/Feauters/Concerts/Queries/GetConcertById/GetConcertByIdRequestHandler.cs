using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Concerts.Queries.Common;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.Queries.GetConcertById;

public class GetConcertByIdRequestHandler : IRequestHandler<GetConcertByIdRequest , GetConcertResponse>
{
    private readonly IConcertsRepository _concertRepository;
    private readonly IMapper _mapper;

    public GetConcertByIdRequestHandler(IConcertsRepository concertRepository, IMapper mapper)
    {
        _concertRepository = concertRepository;
        _mapper = mapper;
    }
    
    public async Task<GetConcertResponse> Handle(GetConcertByIdRequest request, CancellationToken cancellationToken)
    {
        var concertEntity = await _concertRepository.GetConcertByIdAsync(request.Id , request.IsRequestedByAdmin);
        return _mapper.Map<GetConcertResponse>(concertEntity);
    }
}