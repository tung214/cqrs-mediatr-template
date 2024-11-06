using FluentValidation;
using Laborie.Service.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Laborie.Service.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
        
        services.AddMediatR(applicationAssembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}
