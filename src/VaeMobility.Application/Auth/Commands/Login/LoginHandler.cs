using FluentValidation;
using VaeMobility.Application.Auth.Services.Interfaces;
using VaeMobility.Application.Generic;
using VaeMobility.Domain.Auth.Services.Interfaces;
using VaeMobility.Domain.Generic.Repositories.Interfaces;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Application.Auth.Commands.Login;

public sealed class LoginHandler(
    IAuthDomainService authDomainService,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService,
    IRefreshTokenIssuer refreshTokenIssuer,
    IUnitOfWork unitOfWork,
    IValidator<LoginCommand> validator) : ILoginHandler
{
    public async Task<Result<LoginResponse>> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var user = await authDomainService.FindByEmailAsync(command.Email, cancellationToken);
        if (user is null || user.IsDeleted || !passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            return Result<LoginResponse>.Fail(DomainMessages.Auth.CredenciaisInvalidas);
        }

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
