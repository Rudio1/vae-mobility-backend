using VaeMobility.Application.Auth.Commands.Login;
using VaeMobility.Application.Auth.Services.Interfaces;
using VaeMobility.Application.Generic;
using VaeMobility.Domain.Auth.Services.Interfaces;
using VaeMobility.Domain.Generic.Repositories.Interfaces;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Application.Auth.Commands.RefreshToken;

public sealed class RefreshTokenHandler(
    IAuthDomainService authDomainService,
    IJwtTokenService jwtTokenService,
    IRefreshTokenIssuer refreshTokenIssuer,
    IUnitOfWork unitOfWork) : IRefreshTokenHandler
{
    public async Task<Result<LoginResponse>> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            return Result<LoginResponse>.Fail(DomainMessages.Auth.RefreshTokenInvalido);
        }

        var hash = refreshTokenIssuer.Hash(command.RefreshToken);
        var stored = await authDomainService.FindRefreshTokenAsync(hash, cancellationToken);
        if (stored is null || !stored.IsActive)
        {
            return Result<LoginResponse>.Fail(DomainMessages.Auth.RefreshTokenInvalido);
        }

        var user = await authDomainService.FindByIdAsync(stored.UserId, cancellationToken);
        if (user is null || user.IsDeleted)
        {
            return Result<LoginResponse>.Fail(DomainMessages.Auth.RefreshTokenInvalido);
        }

        stored.Revoke();
        var access = jwtTokenService.GenerateAccessToken(user);
        var refresh = refreshTokenIssuer.Issue();
        await authDomainService.IssueRefreshTokenAsync(user.Id, refresh.TokenHash, refresh.ExpiresAt, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LoginResponse>.Ok(new LoginResponse(
            access.AccessToken,
            refresh.Token,
            access.ExpiresAt,
            new AuthUserResponse(user.Id, user.Email, user.Name)));
    }
}
