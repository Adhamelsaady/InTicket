using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;

public class GetPaymentsRequestHandler : IRequestHandler<GetPaymentsRequest , PagedResult<GetPaymentResponse>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentsRequestHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }
    
    public async Task <PagedResult<GetPaymentResponse>> Handle(GetPaymentsRequest request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetAllPaymentsOfUserAsync(request.PaymentResourceParameters, request.UserId);
        var paymentsToReturn = _mapper.Map<List<GetPaymentResponse>>(payments.Items);
        var result = new PagedResult<GetPaymentResponse>()
        {
            TotalCount = payments.TotalCount,
            PageNumber = payments.PageNumber,
            PageSize = payments.PageSize,
            Items = paymentsToReturn
        };
        return result;
    }
}