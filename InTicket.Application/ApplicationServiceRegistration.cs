using System.Reflection;
using InTicket.Application.Contracts;
using InTicket.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InTicket.Application;
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IAuthService , AuthService>();
        return services;
    }
}