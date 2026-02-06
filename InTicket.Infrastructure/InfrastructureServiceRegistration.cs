using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Services;
using InTicket.Infrastructure.Stripe;
using Microsoft.Extensions.DependencyInjection;

namespace InTicket.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEmailService , EmailService>();
        services.AddScoped<IStripePaymentServices , StripePaymentService>();
        return services;
    }
}