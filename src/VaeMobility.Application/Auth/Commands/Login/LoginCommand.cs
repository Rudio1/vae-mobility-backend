using VaeMobility.Application.Generic;

namespace VaeMobility.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password);

public sealed record AuthUserResponse(Guid Id, string Email, string Name);

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    AuthUserResponse User);

public interface ILoginHandler
{
    Task<Result<LoginResponse>> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default);
}
