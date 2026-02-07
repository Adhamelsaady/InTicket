using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.ResourceParameters;
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
        var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
        return payment;
    }

    public async Task UpdateAsync(Payments payment)
    {
        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync();
        Console.WriteLine($"[Repository] Updated payment {payment.PaymentId} - Done: {payment.Done}");
    }

    public async Task<PagedResult<Payments>> GetAllPaymentsOfUserAsync(PaymentResourceParameters paymentResourceParameters , 
        string userId)
    {
        var payments =  _dbContext.Payments.AsQueryable();
        payments = payments.Where(p => p.UserId == userId);
        if (paymentResourceParameters.FromDate != null)
        {
            var fromDateOfCreation = paymentResourceParameters.FromDate.Value.AddHours(1);
            payments = payments.Where(p => p.ExpirationDate >= fromDateOfCreation);
        }
        if (paymentResourceParameters.ToDate != null)
        {
            var toDateOfCreation = paymentResourceParameters.ToDate.Value.AddHours(1);
            payments = payments.Where(p => p.ExpirationDate <= toDateOfCreation);
        }

        if (paymentResourceParameters.isExpired == true)
        {
            payments = payments.Where(p => p.ExpirationDate < DateTime.UtcNow);
        }

        if (paymentResourceParameters.isPaid == true)
        {
            payments = payments.Where(p => p.Done == true);
        }
        var totalCount = await payments.CountAsync();
        var paymentsList = await payments
            .Skip((paymentResourceParameters.PageNumber - 1) * paymentResourceParameters.PageSize)
            .Take(paymentResourceParameters.PageSize).ToListAsync();
        return new PagedResult<Payments>
        {
            Items = paymentsList,
            TotalCount = totalCount,
            PageNumber = paymentResourceParameters.PageNumber,
            PageSize = paymentResourceParameters.PageSize
        };
    }
}