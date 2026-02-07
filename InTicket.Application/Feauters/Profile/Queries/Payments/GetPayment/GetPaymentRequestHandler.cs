using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Queries.GetPayment;

public class GetPaymentRequestHandler : IRequestHandler<GetPaymentRequest , GetPaymentResponse>
{
    private readonly IBaseRepository<Payments> _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentRequestHandler(IBaseRepository<Payments>  paymentRepository , IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }
    public async Task<GetPaymentResponse> Handle(GetPaymentRequest request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId);
        if (payment == null || payment.UserId != request.UserId)
            throw new Exception("Payment not found");
        var paymentToReturn = _mapper.Map<GetPaymentResponse>(payment);
        return paymentToReturn;
    }
}