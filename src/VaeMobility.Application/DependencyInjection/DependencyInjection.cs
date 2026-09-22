using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VaeMobility.Application.Auth.Commands.Login;
using VaeMobility.Application.Auth.Commands.Logout;
using VaeMobility.Application.Auth.Commands.RefreshToken;
using VaeMobility.Application.Auth.Queries.GetMe;
using VaeMobility.Application.Auth.Services;
using VaeMobility.Application.Auth.Services.Interfaces;
using VaeMobility.Application.Catalog.Services;
using VaeMobility.Application.Catalog.Services.Interfaces;
using VaeMobility.Application.Catalog.Validators;
using VaeMobility.Application.Media.Commands.UploadMedia;
using VaeMobility.Domain.Auth.Services;
using VaeMobility.Domain.Auth.Services.Interfaces;
using VaeMobility.Domain.Catalog.Services;
using VaeMobility.Domain.Catalog.Services.Interfaces;

namespace VaeMobility.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<SaveProductRequestValidator>();

        services.AddScoped<IAuthDomainService, AuthDomainService>();
        services.AddScoped<ICatalogDomainService, CatalogDomainService>();
        services.AddScoped<IRefreshTokenIssuer, RefreshTokenIssuer>();

        services.AddScoped<ILoginHandler, LoginHandler>();
        services.AddScoped<IRefreshTokenHandler, RefreshTokenHandler>();
        services.AddScoped<ILogoutHandler, LogoutHandler>();
        services.AddScoped<IGetMeHandler, GetMeHandler>();
        services.AddScoped<ICatalogAppService, CatalogAppService>();
        services.AddScoped<IUploadMediaHandler, UploadMediaHandler>();

        return services;
    }
}
