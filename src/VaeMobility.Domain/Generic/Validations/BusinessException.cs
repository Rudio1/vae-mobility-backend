namespace VaeMobility.Domain.Generic.Validations;

public sealed class BusinessException : DomainException
{
    public BusinessException(string message) : base(message)
    {
    }
}
