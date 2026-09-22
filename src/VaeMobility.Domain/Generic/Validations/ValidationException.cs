namespace VaeMobility.Domain.Generic.Validations;

public sealed class ValidationException : DomainException
{
    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("Validation failed.")
    {
        Errors = errors;
    }

    public IReadOnlyDictionary<string, string[]>? Errors { get; }
}
