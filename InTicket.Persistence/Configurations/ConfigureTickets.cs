using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Configurations;

public static class TicketsConfiguration
{
    public static void ConfigureTickets(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .UseTpcMappingStrategy();
         
        modelBuilder.Entity<MatchTicket>()
            .ToTable("MatchTickets");
        
        modelBuilder.Entity<ConcertTicket>()
            .ToTable("EventTickets");
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasOne(t => t.Holder)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.HolderId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.Property(t => t.Status)
                .IsRequired()
                .HasDefaultValue(TicketStatus.Open);
        });
    }
}