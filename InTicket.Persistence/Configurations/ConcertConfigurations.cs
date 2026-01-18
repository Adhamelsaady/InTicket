using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Configurations;

public static class ConcertConfigurations
{
    public static void ConfigureConcerts(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Concert>()
            .ToTable("Concerts");
        
        modelBuilder.Entity<Concert>().HasData(ConcertSeed.Concerts);
    }
}