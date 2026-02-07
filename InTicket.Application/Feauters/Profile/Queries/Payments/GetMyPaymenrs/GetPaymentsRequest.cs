using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.ResourceParameters;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;

public class GetPaymentsRequest : IRequest<PagedResult<GetPaymentResponse>>
{
    public PaymentResourceParameters PaymentResourceParameters { get; set; }
    public string? UserId { get; set; }
}