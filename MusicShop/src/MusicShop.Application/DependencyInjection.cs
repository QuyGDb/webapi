using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;

namespace MusicShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 1. Register MediatR for the entire Application Layer
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // 2. Register FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
