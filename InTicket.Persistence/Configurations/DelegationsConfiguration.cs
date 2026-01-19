using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Configurations;

public static class DelegationsConfiguration
{
    public static void ConfigureDelegations(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delegation>(entity =>
        {
            entity.ToTable("Delegations"); 
            entity.HasKey(d => d.Id);
            entity.HasOne(d => d.Delegator)
                .WithOne(u => u.DelegationGiven)
                .HasForeignKey<Delegation>(d => d.DelegatorId)
                .OnDelete(DeleteBehavior.NoAction)  
                .IsRequired(); 
            entity.HasOne(d => d.Delegate)
                .WithMany(u => u.DelegationsReceived)
                .HasForeignKey(d => d.DelegateId)
                .OnDelete(DeleteBehavior.NoAction)  
                .IsRequired(); 
        });
    }
}