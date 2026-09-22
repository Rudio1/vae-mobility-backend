using Microsoft.Extensions.Options;

namespace VaeMobility.WebApi.Auth;

public interface IAuthCookieService
{
    void SetTokens(HttpResponse response, string accessToken, string refreshToken, DateTime accessExpiresAt, DateTime refreshExpiresAt);
    void ClearTokens(HttpResponse response);
    string? GetAccessToken(HttpRequest request);
    string? GetRefreshToken(HttpRequest request);
}

public sealed class AuthCookieService(IOptions<AuthCookieOptions> options) : IAuthCookieService
{
    public void SetTokens(
        HttpResponse response,
        string accessToken,
        string refreshToken,
        DateTime accessExpiresAt,
        DateTime refreshExpiresAt)
    {
        response.Cookies.Append(options.Value.AccessTokenName, accessToken, Build(accessExpiresAt));
        response.Cookies.Append(options.Value.RefreshTokenName, refreshToken, Build(refreshExpiresAt));
    }

    public void ClearTokens(HttpResponse response)
    {
        response.Cookies.Delete(options.Value.AccessTokenName, Build(DateTime.UtcNow.AddDays(-1)));
        response.Cookies.Delete(options.Value.RefreshTokenName, Build(DateTime.UtcNow.AddDays(-1)));
    }

    public string? GetAccessToken(HttpRequest request)
        => request.Cookies.TryGetValue(options.Value.AccessTokenName, out var token) ? token : null;

    public string? GetRefreshToken(HttpRequest request)
        => request.Cookies.TryGetValue(options.Value.RefreshTokenName, out var token) ? token : null;

    private CookieOptions Build(DateTime expiresAt)
    {
        var sameSite = options.Value.SameSite.Equals("None", StringComparison.OrdinalIgnoreCase)
            ? SameSiteMode.None
            : SameSiteMode.Lax;

        return new CookieOptions
        {
            HttpOnly = true,
            Secure = options.Value.Secure || sameSite == SameSiteMode.None,
            SameSite = sameSite,
            Path = options.Value.Path,
            Expires = expiresAt
        };
    }
}
