using InTicket.Domain;
using InTicket.Persistence.Configurations;
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
    public DbSet<Delegation> Delegations { get; set; }

    public DbSet<ConcertTicket> ConcertTickets { get; set; }
    
    public DbSet<MatchTicket> MatchTickets { get; set; }
    
    public DbSet<Ticket> Tickets { get; set; }
    
    public DbSet<Payments> Payments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureBaseEvent();
        modelBuilder.ConfigureMatches();
        modelBuilder.ConfigureConcerts();
        modelBuilder.ConfigureDelegations();
        modelBuilder.ConfigureTeams();
        modelBuilder.ConfigureTickets();
        modelBuilder.ConfigureConcertTickets();
        modelBuilder.ConfigureMatchTickets();
        modelBuilder.ConfigurePayments();
    }
}