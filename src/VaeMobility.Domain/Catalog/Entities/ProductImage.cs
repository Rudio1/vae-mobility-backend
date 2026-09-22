using VaeMobility.Domain.Catalog.ValueObjects;

namespace VaeMobility.Domain.Catalog.Entities;

public sealed class ProductImage
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public string Src { get; private set; } = string.Empty;
    public string Alt { get; private set; } = string.Empty;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsCard { get; private set; }

    private ProductImage()
    {
    }

    public static ProductImage Create(Guid productId, CatalogImage image, int sortOrder, bool isCard)
    {
        return new ProductImage
        {
            ProductId = productId,
            Src = image.Src,
            Alt = image.Alt,
            Width = image.Width,
            Height = image.Height,
            SortOrder = sortOrder,
            IsCard = isCard
        };
    }

    public CatalogImage ToImage() => new(Src, Alt, Width, Height);
}
