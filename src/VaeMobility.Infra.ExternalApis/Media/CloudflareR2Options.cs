namespace VaeMobility.Infra.ExternalApis.Media;

public sealed class CloudflareR2Options
{
    public const string SectionName = "Cloudflare:R2";

    public string AccountId { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string Bucket { get; init; } = string.Empty;
    public string ServiceUrl { get; init; } = string.Empty;
    public string PublicBaseUrl { get; init; } = string.Empty;
}
