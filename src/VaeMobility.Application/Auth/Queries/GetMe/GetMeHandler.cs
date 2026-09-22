using VaeMobility.Application.Auth.Commands.Login;
using VaeMobility.Application.Generic;
using VaeMobility.Application.Generic.Services.Interfaces;
using VaeMobility.Domain.Auth.Services.Interfaces;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Application.Auth.Queries.GetMe;

public sealed class GetMeHandler(
    ICurrentUserContext currentUserContext,
    IAuthDomainService authDomainService) : IGetMeHandler
{
    public async Task<Result<AuthUserResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        if (currentUserContext.UserId is not Guid userId)
        {
            return Result<AuthUserResponse>.Fail(DomainMessages.Auth.UsuarioNaoEncontrado);
        }

        var user = await authDomainService.FindByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result<AuthUserResponse>.Fail(DomainMessages.Auth.UsuarioNaoEncontrado);
        }

        return Result<AuthUserResponse>.Ok(new AuthUserResponse(user.Id, user.Email, user.Name));
    }
}
