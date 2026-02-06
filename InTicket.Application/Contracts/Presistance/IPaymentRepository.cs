using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IPaymentRepository
{
    Task<Payments> GetPaymentByPaymentCodeAsync(Guid paymentCode);
    Task<Payments> GetPaymentByIntentAsync(string paymentIntentId);
}