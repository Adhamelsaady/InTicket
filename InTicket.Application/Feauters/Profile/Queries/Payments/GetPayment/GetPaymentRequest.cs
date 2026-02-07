using InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Queries.GetPayment;

public class GetPaymentRequest : IRequest<GetPaymentResponse>
{
    public Guid PaymentId { get; set; }
    public string UserId { get; set; }
}