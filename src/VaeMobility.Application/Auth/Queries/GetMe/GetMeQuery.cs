using VaeMobility.Application.Auth.Commands.Login;
using VaeMobility.Application.Generic;

namespace VaeMobility.Application.Auth.Queries.GetMe;

public interface IGetMeHandler
{
    Task<Result<AuthUserResponse>> HandleAsync(CancellationToken cancellationToken = default);
}
