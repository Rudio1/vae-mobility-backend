using VaeMobility.Domain.Catalog.ValueObjects;

namespace VaeMobility.Domain.Catalog.Entities;

public sealed class CategoryImage
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid CategoryId { get; private set; }
    public string Slot { get; private set; } = string.Empty;
    public string Src { get; private set; } = string.Empty;
    public string Alt { get; private set; } = string.Empty;
    public int Width { get; private set; }
    public int Height { get; private set; }

    private CategoryImage()
    {
    }

    public static CategoryImage Create(Guid categoryId, string slot, CatalogImage image)
    {
        return new CategoryImage
        {
            CategoryId = categoryId,
            Slot = slot,
            Src = image.Src,
            Alt = image.Alt,
            Width = image.Width,
            Height = image.Height
        };
    }

    public CatalogImage ToImage() => new(Src, Alt, Width, Height);
}
