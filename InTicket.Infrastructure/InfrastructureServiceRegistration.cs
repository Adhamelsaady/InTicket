using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InTicket.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEmailService , EmailService>();
        return services;
    }
}