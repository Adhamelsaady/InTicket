using System.Reflection;
using InTicket.Application.Contracts;
using InTicket.Application.Contracts.Infrasructure;
using InTicket.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace InTicket.Application;
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IJwtTokenGeneration , JwtTokenGeneration>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}