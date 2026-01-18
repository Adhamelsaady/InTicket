using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Configurations;

public static class BaseEventConfiguration 
{
    public static void ConfigureBaseEvent(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseEvent>()
            .UseTpcMappingStrategy();
    }
}