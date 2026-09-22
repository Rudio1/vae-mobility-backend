using VaeMobility.Domain.Catalog.Enums;
using VaeMobility.Domain.Catalog.ValueObjects;
using VaeMobility.Domain.Generic.Entities;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Domain.Catalog.Entities;

public sealed class Category : AggregateRoot
{
    private readonly List<CategoryImage> _images = [];

    public string Slug { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string ShortDescription { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }
    public string? SeoTitle { get; private set; }
    public string? SeoDescription { get; private set; }
    public string? SeoOgImage { get; private set; }

    public IReadOnlyCollection<CategoryImage> Images => _images;

    private Category()
    {
    }

    public static Category Create(
        string name,
        string slug,
        string shortDescription,
        string description,
        int sortOrder,
        string? seoTitle,
        string? seoDescription,
        string? seoOgImage,
        IReadOnlyDictionary<string, CatalogImage> images)
    {
        var category = new Category();
        category.Apply(name, CatalogSlug.Normalize(slug), shortDescription, description, sortOrder, seoTitle, seoDescription, seoOgImage);
        category.ReplaceImages(images);
        category.EnsureExplore();
        return category;
    }

    public void Update(
        string name,
        string shortDescription,
        string description,
        int sortOrder,
        string? seoTitle,
        string? seoDescription,
        string? seoOgImage,
        IReadOnlyDictionary<string, CatalogImage> images)
    {
        Apply(name, Slug, shortDescription, description, sortOrder, seoTitle, seoDescription, seoOgImage);
        ReplaceImages(images);
        EnsureExplore();
        MarkAsUpdated();
    }

    public CatalogImage? GetImage(string slot)
        => _images.FirstOrDefault(image => image.Slot == slot)?.ToImage();

    public bool HasExplore => _images.Any(image => image.Slot == CategoryImageSlots.Explore);

    private void Apply(
        string name,
        string slug,
        string shortDescription,
        string description,
        int sortOrder,
        string? seoTitle,
        string? seoDescription,
        string? seoOgImage)
    {
        if (seoTitle is { Length: > 60 })
        {
            throw new BusinessException(DomainMessages.Catalog.SeoTitleMaximo);
        }

        if (seoDescription is { Length: > 160 })
        {
            throw new BusinessException(DomainMessages.Catalog.SeoDescriptionMaximo);
        }

        Name = name.Trim();
        Slug = slug;
        ShortDescription = shortDescription.Trim();
        Description = description.Trim();
        SortOrder = sortOrder;
        SeoTitle = string.IsNullOrWhiteSpace(seoTitle) ? null : seoTitle.Trim();
        SeoDescription = string.IsNullOrWhiteSpace(seoDescription) ? null : seoDescription.Trim();
        SeoOgImage = string.IsNullOrWhiteSpace(seoOgImage) ? null : seoOgImage.Trim();
    }

    private void ReplaceImages(IReadOnlyDictionary<string, CatalogImage> images)
    {
        _images.Clear();
        foreach (var (slot, image) in images)
        {
            _images.Add(CategoryImage.Create(Id, slot, image));
        }
    }

    private void EnsureExplore()
    {
        if (!HasExplore)
        {
            throw new BusinessException(DomainMessages.Catalog.ExploreObrigatorio);
        }
    }
}
