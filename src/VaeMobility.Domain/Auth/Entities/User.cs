using VaeMobility.Domain.Generic.Entities;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Domain.Auth.Entities;

public sealed class User : AggregateRoot
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    private User()
    {
    }

    public static User Create(string email, string passwordHash, string name)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new BusinessException(DomainMessages.Auth.CredenciaisInvalidas);
        }

        return new User
        {
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            Name = name.Trim()
        };
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        MarkAsUpdated();
    }
}
