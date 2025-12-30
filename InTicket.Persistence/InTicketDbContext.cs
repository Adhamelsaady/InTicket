using InTicket.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence;
 
public class InTicketDbContext : IdentityDbContext<ApplicationUser>
{
    public InTicketDbContext(DbContextOptions<InTicketDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<BaseEvent> BaseEvents { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Concert> Concerts { get; set; }
    public DbSet<Team> Teams { get; set; }
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BaseEvent>()
            .UseTpcMappingStrategy();
        modelBuilder.Entity<Match>()
            .ToTable("Matches");
        modelBuilder.Entity<Concert>()
            .ToTable("Concerts");
        modelBuilder.Entity<Match>(entity =>
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
        modelBuilder.Entity<Team>().HasData(TeamSeed.PremierLeagueTeams);
        modelBuilder.Entity<Match>().HasData(MatchSeed.PremierLeagueMatches);
        modelBuilder.Entity<Concert>().HasData(ConcertSeed.Concerts);
    }
}