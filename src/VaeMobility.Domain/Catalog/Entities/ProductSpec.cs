namespace VaeMobility.Domain.Catalog.Entities;

public sealed class ProductSpec
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public string? Group { get; private set; }
    public int SortOrder { get; private set; }

    private ProductSpec()
    {
    }

    public static ProductSpec Create(Guid productId, string label, string value, string? group, int sortOrder)
        => new()
        {
            ProductId = productId,
            Label = label.Trim(),
            Value = value.Trim(),
            Group = string.IsNullOrWhiteSpace(group) ? null : group.Trim(),
            SortOrder = sortOrder
        };
}
