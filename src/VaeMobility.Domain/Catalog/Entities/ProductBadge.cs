namespace VaeMobility.Domain.Catalog.Entities;

public sealed class ProductBadge
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public string Badge { get; private set; } = string.Empty;

    private ProductBadge()
    {
    }

    public static ProductBadge Create(Guid productId, string badge)
        => new() { ProductId = productId, Badge = badge };
}
