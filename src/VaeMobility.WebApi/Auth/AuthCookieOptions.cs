namespace VaeMobility.WebApi.Auth;

public sealed class AuthCookieOptions
{
    public const string SectionName = "Auth:Cookies";

    public string AccessTokenName { get; init; } = "vaemobility_access_token";
    public string RefreshTokenName { get; init; } = "vaemobility_refresh_token";
    public string SameSite { get; init; } = "Lax";
    public bool Secure { get; init; }
    public string Path { get; init; } = "/";
}
