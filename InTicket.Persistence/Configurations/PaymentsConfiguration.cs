using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Configurations;

public static class PaymentsConfiguration
{
    public static void ConfigurePayments(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payments> (p =>
        {
            p.HasKey(payments => payments.PaymentId);
            p.HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(u => u.UserId);
        });
    }
}