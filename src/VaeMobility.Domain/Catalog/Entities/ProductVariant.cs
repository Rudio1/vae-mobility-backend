using VaeMobility.Domain.Catalog.ValueObjects;

namespace VaeMobility.Domain.Catalog.Entities;

public sealed class ProductVariant
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public string? Swatch { get; private set; }
    public string? ImageSrc { get; private set; }
    public string? ImageAlt { get; private set; }
    public int? ImageWidth { get; private set; }
    public int? ImageHeight { get; private set; }
    public int SortOrder { get; private set; }

    private ProductVariant()
    {
    }

    public static ProductVariant Create(
        Guid productId,
        string name,
        string type,
        string value,
        string? swatch,
        CatalogImage? image,
        int sortOrder)
    {
        return new ProductVariant
        {
            ProductId = productId,
            Name = name.Trim(),
            Type = type,
            Value = value.Trim(),
            Swatch = swatch,
            ImageSrc = image?.Src,
            ImageAlt = image?.Alt,
            ImageWidth = image?.Width,
            ImageHeight = image?.Height,
            SortOrder = sortOrder
        };
    }

    public CatalogImage? Image =>
        ImageSrc is null || ImageAlt is null || ImageWidth is null || ImageHeight is null
            ? null
            : new CatalogImage(ImageSrc, ImageAlt, ImageWidth.Value, ImageHeight.Value);
}
