using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly InTicketDbContext _dbContext;
    private IBaseRepository<Payments> _baseRepositoryImplementation;

    public PaymentRepository(InTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Payments> GetPaymentByPaymentCodeAsync(Guid paymentCode)
    {
        return await _dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId ==  paymentCode);
    }
    public async Task<Payments> GetPaymentByIntentAsync(string paymentIntentId)
    {
        var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.StripePaymentIntentId == paymentIntentId);
        return payment;
    }
}