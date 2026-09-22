using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaeMobility.Application.Auth.Commands.Login;
using VaeMobility.Application.Auth.Commands.Logout;
using VaeMobility.Application.Auth.Commands.RefreshToken;
using VaeMobility.Application.Auth.Queries.GetMe;
using VaeMobility.WebApi.Auth;
using VaeMobility.WebApi.Controllers.Generic;

namespace VaeMobility.WebApi.Controllers.Auth;

[Route("api/auth")]
public sealed class AuthController(
    ILoginHandler loginHandler,
    IRefreshTokenHandler refreshTokenHandler,
    ILogoutHandler logoutHandler,
    IGetMeHandler getMeHandler,
    IAuthCookieService authCookieService,
    IConfiguration configuration) : ApiControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await loginHandler.HandleAsync(command, cancellationToken);
        if (!result.Success || result.Value is null)
        {
            return FromResult(result);
        }

        SetAuthCookies(result.Value);
        return OkResponse(result.Value);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand? command, CancellationToken cancellationToken)
    {
        var refreshToken = command?.RefreshToken ?? authCookieService.GetRefreshToken(Request);
        var result = await refreshTokenHandler.HandleAsync(new RefreshTokenCommand(refreshToken ?? string.Empty), cancellationToken);
        if (!result.Success || result.Value is null)
        {
            return FromResult(result);
        }

        SetAuthCookies(result.Value);
        return OkResponse(result.Value);
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand? command, CancellationToken cancellationToken)
    {
        var refreshToken = command?.RefreshToken ?? authCookieService.GetRefreshToken(Request);
        var result = await logoutHandler.HandleAsync(new LogoutCommand(refreshToken), cancellationToken);
        authCookieService.ClearTokens(Response);
        return FromResult(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
        => FromResult(await getMeHandler.HandleAsync(cancellationToken));

    private void SetAuthCookies(LoginResponse session)
    {
        var refreshDays = configuration.GetValue("Jwt:RefreshTokenExpirationDays", 7);
        authCookieService.SetTokens(
            Response,
            session.AccessToken,
            session.RefreshToken,
            session.ExpiresAt,
            DateTime.UtcNow.AddDays(refreshDays));
    }
}
