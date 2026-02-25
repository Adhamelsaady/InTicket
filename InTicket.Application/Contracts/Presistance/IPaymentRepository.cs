using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IPaymentRepository
{
    Task<Payment> GetPaymentByPaymentCodeAsync(Guid paymentCode);
    Task<Payment> GetPaymentByIntentAsync(string paymentIntentId);
    Task UpdateAsync(Payment payment);

    Task<PagedResult<Payment>> GetAllPaymentsOfUserAsync(PaymentResourceParameters paymentResourceParameters,
        string userId);
}