namespace VaeMobility.Domain.Auth.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private RefreshToken()
    {
    }

    public static RefreshToken Issue(Guid userId, string tokenHash, DateTime expiresAt)
    {
        return new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = expiresAt
        };
    }

    public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;

    public void Revoke()
    {
        RevokedAt ??= DateTime.UtcNow;
    }
}
