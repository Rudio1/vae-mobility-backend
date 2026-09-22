using Microsoft.Extensions.DependencyInjection;

namespace VaeMobility.Infra.Jobs;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraJobs(this IServiceCollection services)
    {
        return services;
    }
}
