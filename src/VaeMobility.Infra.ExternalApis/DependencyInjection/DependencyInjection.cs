using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaeMobility.Application.Media.Services.Interfaces;
using VaeMobility.Infra.ExternalApis.Media;

namespace VaeMobility.Infra.ExternalApis;

public static class DependencyInjection
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudflareR2Options>(configuration.GetSection(CloudflareR2Options.SectionName));
        services.AddSingleton<IMediaStorage, CloudflareR2MediaStorage>();
        return services;
    }
}
