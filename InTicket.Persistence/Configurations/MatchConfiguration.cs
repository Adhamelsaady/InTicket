
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace InTicket.Persistence.Configurations;

public static class MatchConfiguration 
{
    public static void ConfigureMatches(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InTicket.Domain.Match>()
            .ToTable("Matches");
        modelBuilder.Entity<InTicket.Domain.Match>(entity =>
        {
            entity.HasOne(m => m.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<InTicket.Domain.Match>().HasData(MatchSeed.PremierLeagueMatches);
    }
}