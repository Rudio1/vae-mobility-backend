namespace VaeMobility.Domain.Catalog.Entities;

public sealed class ProductHighlight
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }

    private ProductHighlight()
    {
    }

    public static ProductHighlight Create(Guid productId, string text, int sortOrder)
        => new() { ProductId = productId, Text = text.Trim(), SortOrder = sortOrder };
}
