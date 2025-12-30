using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Concerts.Queries.Common;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.Queries.GetAllConcerts;

public class GetAllConcertsRequestHandler : IRequestHandler<GetAllConcertsRequest, PagedResult<GetConcertResponse>>
{
    
    private readonly IConcertsRepository _concertsRepository;
    private readonly IMapper _mapper;
    public GetAllConcertsRequestHandler(IConcertsRepository concertsRepository, IMapper mapper)
    {
        _concertsRepository = concertsRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetConcertResponse>> Handle(GetAllConcertsRequest request,CancellationToken cancellationToken)
    {
        var concerts = await _concertsRepository.GetAllConcerts(request.ConcertResourceParameters);
        var concertsToReturn = _mapper.Map<List<GetConcertResponse>>(concerts.Items);
        var result = new PagedResult<GetConcertResponse>();
        result.TotalCount = concerts.TotalCount;
        result.PageNumber = concerts.PageNumber;
        result.PageSize = concerts.PageSize;
        result.Items = concertsToReturn;
        return result;
    }
    
    
}