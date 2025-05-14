using System.Reflection;
using Application.ProcessingServices;
using Application.Services.Foundations;
using Infrastructure.ProcessingServices;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DIRegistrations
{
    public static void AddTranslationServices(this IServiceCollection services)
    {
        services.AddSingleton<ITranslationService, TranslationService>();
    }

    public static void AddRepositories(this IServiceCollection services, Assembly givenAssembly)
    {
        var repositories = givenAssembly.DefinedTypes
            .Where(type => !type.IsAbstract && !type.IsInterface && // Non-abstract, non-interface classes
                           type.GetInterfaces().Any(i => i.IsGenericType && // Implements a generic interface
                                                        i.GetGenericTypeDefinition() == typeof(IRepository<,>)))
            .ToList();

        foreach (var type in repositories)
        {
            // Find the specific IValidator<T, TId> interface implemented by this type
            var repositoryInterface = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>));

            // Register the type as a singleton for the specific closed generic interface
            services.AddScoped(repositoryInterface, type);
        }
    }

    public static void AddFoundationServices(this IServiceCollection services, Assembly givenAssembly)
    {
        var foundationServices = givenAssembly.DefinedTypes
            .Where(type => !type.IsAbstract && !type.IsInterface && // Non-abstract, non-interface classes
                           type.GetInterfaces().Any(i => i.IsGenericType && // Implements a generic interface
                                                        i.GetGenericTypeDefinition() == typeof(IBaseService<,>)))
            .ToList();

        foreach (var type in foundationServices)
        {
            // Find the specific IValidator<T, TId> interface implemented by this type
            var foundationServicesInterface = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseService<,>));

            // Register the type as a singleton for the specific closed generic interface
            services.AddScoped(foundationServicesInterface, type);
        }
    }
}