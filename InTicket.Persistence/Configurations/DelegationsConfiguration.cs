using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Configurations;

public static class DelegationsConfiguration
{
    public static void ConfigureDelegations(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delegation>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasOne(d => d.Delegator)
                .WithMany(u => u.DelegationsGiven)
                .HasForeignKey(d => d.DelegatorId)
                .OnDelete(DeleteBehavior.Restrict);  
            entity.HasOne(d => d.Delegate)
                .WithMany(u => u.DelegationsReceived)
                .HasForeignKey(d => d.DelegateId)
                .OnDelete(DeleteBehavior.Restrict);  
        });
    }
}