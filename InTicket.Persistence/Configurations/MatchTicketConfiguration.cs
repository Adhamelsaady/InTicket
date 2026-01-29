using InTicket.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InTicket.Persistence.Configurations;

public static class MatchTicketConfiguration
{
    public static void ConfigureMatchTickets(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MatchTicket>(entity =>
        {
            entity.HasOne(mt => mt.Match)
                .WithMany(m => m.MatchTickets)
                .HasForeignKey(mt => mt.MatchId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(mt => mt.MatchId)
                .IsRequired();
        });
    }
}