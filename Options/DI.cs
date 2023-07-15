namespace SagaOrchestrator.Options;

public static class DI
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationOptions>(configuration);
        return services;
    }
}