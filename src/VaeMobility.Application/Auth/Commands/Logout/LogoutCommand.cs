using VaeMobility.Application.Generic;

namespace VaeMobility.Application.Auth.Commands.Logout;

public sealed record LogoutCommand(string? RefreshToken);

public interface ILogoutHandler
{
    Task<Result> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default);
}
