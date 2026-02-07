using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IPaymentRepository
{
    Task<Payments> GetPaymentByPaymentCodeAsync(Guid paymentCode);
    Task<Payments> GetPaymentByIntentAsync(string paymentIntentId);
    Task UpdateAsync(Payments payment);

    Task<PagedResult<Payments>> GetAllPaymentsOfUserAsync(PaymentResourceParameters paymentResourceParameters,
        string userId);
}