using VaeMobility.Application.Auth.Commands.Login;
using VaeMobility.Application.Generic;

namespace VaeMobility.Application.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken);

public interface IRefreshTokenHandler
{
    Task<Result<LoginResponse>> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default);
}
