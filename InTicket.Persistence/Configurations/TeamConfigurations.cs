using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Configurations;

public static class TeamConfigurations
{
    public static void ConfigureTeams(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>().HasData(TeamSeed.PremierLeagueTeams);
    }
}