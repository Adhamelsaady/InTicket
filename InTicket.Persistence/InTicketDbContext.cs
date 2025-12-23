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
}