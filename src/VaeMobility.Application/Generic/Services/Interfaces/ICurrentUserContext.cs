namespace VaeMobility.Application.Generic.Services.Interfaces;

public interface ICurrentUserContext
{
    Guid? UserId { get; }
    bool IsAuthenticated { get; }
}

public sealed class NullCurrentUserContext : ICurrentUserContext
{
    public Guid? UserId => null;
    public bool IsAuthenticated => false;
}
