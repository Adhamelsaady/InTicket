using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Configurations;

public static class ConcertTicketConfiguration
{
    public static void ConfigureConcertTickets(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConcertTicket>(entity =>
        {
            entity.HasOne(ct => ct.Concert)
                .WithMany(c => c.ConcertTickets).
                HasForeignKey(ct => ct.ConcertId).OnDelete(DeleteBehavior.Restrict);
            entity.Property(ct => ct.ConcertId).IsRequired();
        });
    }
}