using VaeMobility.Application.Auth.Services.Interfaces;
using VaeMobility.Application.Generic;
using VaeMobility.Domain.Auth.Services.Interfaces;
using VaeMobility.Domain.Generic.Repositories.Interfaces;

namespace VaeMobility.Application.Auth.Commands.Logout;

public sealed class LogoutHandler(
    IAuthDomainService authDomainService,
    IRefreshTokenIssuer refreshTokenIssuer,
    IUnitOfWork unitOfWork) : ILogoutHandler
{
    public async Task<Result> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            return Result.Ok();
        }

        var stored = await authDomainService.FindRefreshTokenAsync(
            refreshTokenIssuer.Hash(command.RefreshToken),
            cancellationToken);

        if (stored is not null && stored.IsActive)
        {
            stored.Revoke();
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Ok();
    }
}
