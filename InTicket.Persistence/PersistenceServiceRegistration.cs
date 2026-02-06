using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using InTicket.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InTicket.Persistence;

public static class PersistenceServiceRegistration
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<InTicketDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IConcertsRepository, ConcertsRepository>();
        services.AddScoped<IDelegationsRepository, DelegationsRepository>();
        services.AddScoped<IMatchTicketRepository , MatchTicketRepository>();
    }
}