using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaeMobility.Application.Auth.Services.Interfaces;
using VaeMobility.Domain.Auth.Repositories.Interfaces;
using VaeMobility.Domain.Catalog.Repositories.Interfaces;
using VaeMobility.Domain.Generic.Repositories.Interfaces;
using VaeMobility.Infra.Data.Auth;
using VaeMobility.Infra.Data.Auth.Repositories;
using VaeMobility.Infra.Data.Catalog.Repositories;
using VaeMobility.Infra.Data.Context;

namespace VaeMobility.Infra.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
