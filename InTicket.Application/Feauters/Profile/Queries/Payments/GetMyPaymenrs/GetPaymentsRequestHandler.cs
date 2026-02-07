using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;

public class GetPaymentsRequestHandler : IRequestHandler<GetPaymentsRequest , PagedResult<GetPaymentsResponse>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentsRequestHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }
    
    public async Task <PagedResult<GetPaymentsResponse>> Handle(GetPaymentsRequest request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetAllPaymentsOfUserAsync(request.PaymentResourceParameters, request.UserId);
        var paymentsToReturn = _mapper.Map<List<GetPaymentsResponse>>(payments);
        var result = new PagedResult<GetPaymentsResponse>();
        result.TotalCount = payments.TotalCount;
        result.PageNumber = payments.PageNumber;
        result.PageSize = payments.PageSize;
        result.Items = paymentsToReturn;
        return result;
    }
}