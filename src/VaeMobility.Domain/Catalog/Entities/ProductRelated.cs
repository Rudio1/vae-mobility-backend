namespace VaeMobility.Domain.Catalog.Entities;

public sealed class ProductRelated
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public Guid RelatedProductId { get; private set; }
    public int SortOrder { get; private set; }

    private ProductRelated()
    {
    }

    public static ProductRelated Create(Guid productId, Guid relatedProductId, int sortOrder)
        => new() { ProductId = productId, RelatedProductId = relatedProductId, SortOrder = sortOrder };
}
