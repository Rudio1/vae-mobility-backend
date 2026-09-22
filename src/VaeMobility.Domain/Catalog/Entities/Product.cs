using VaeMobility.Domain.Catalog.Enums;
using VaeMobility.Domain.Catalog.ValueObjects;
using VaeMobility.Domain.Generic.Entities;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Domain.Catalog.Entities;

public sealed class Product : AggregateRoot
{
    private readonly List<ProductImage> _images = [];
    private readonly List<ProductHighlight> _highlights = [];
    private readonly List<ProductSpec> _specs = [];
    private readonly List<ProductVariant> _variants = [];
    private readonly List<ProductBadge> _badges = [];
    private readonly List<ProductRelated> _related = [];

    public Guid CategoryId { get; private set; }
    public string Slug { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Status { get; private set; } = ProductStatuses.Draft;
    public bool Featured { get; private set; }
    public int SortOrder { get; private set; }
    public string Summary { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal? PriceAmount { get; private set; }
    public string? Currency { get; private set; }
    public int? WarrantyMonths { get; private set; }
    public string? SeoTitle { get; private set; }
    public string? SeoDescription { get; private set; }
    public string? SeoOgImage { get; private set; }

    public IReadOnlyCollection<ProductImage> Images => _images;
    public IReadOnlyCollection<ProductHighlight> Highlights => _highlights;
    public IReadOnlyCollection<ProductSpec> Specs => _specs;
    public IReadOnlyCollection<ProductVariant> Variants => _variants;
    public IReadOnlyCollection<ProductBadge> Badges => _badges;
    public IReadOnlyCollection<ProductRelated> Related => _related;

    private Product()
    {
    }

    public static Product Create(ProductDraft draft)
    {
        var product = new Product();
        product.Apply(draft, allowSlugChange: true);
        return product;
    }

    public void Update(ProductDraft draft)
    {
        var slugLocked = Status == ProductStatuses.Active;
        if (slugLocked && CatalogSlug.Normalize(draft.Slug) != Slug)
        {
            throw new BusinessException(DomainMessages.Catalog.SlugImutavel);
        }

        Apply(draft, allowSlugChange: !slugLocked);
        MarkAsUpdated();
    }

    public IReadOnlyList<CatalogImage> GalleryImages()
        => _images.Where(image => !image.IsCard).OrderBy(image => image.SortOrder).Select(image => image.ToImage()).ToArray();

    public CatalogImage? CardImage()
        => _images.FirstOrDefault(image => image.IsCard)?.ToImage();

    public void EnsureReadyToPublish(bool categoryHasExplore)
    {
        if (_highlights.Count != 3)
        {
            throw new BusinessException(DomainMessages.Catalog.HighlightsObrigatorios);
        }

        if (!_images.Any(image => !image.IsCard))
        {
            throw new BusinessException(DomainMessages.Catalog.ImagemObrigatoria);
        }

        if (!categoryHasExplore)
        {
            throw new BusinessException(DomainMessages.Catalog.CategoriaSemExplore);
        }
    }

    private void Apply(ProductDraft draft, bool allowSlugChange)
    {
        if (draft.Summary.Trim().Length > 160)
        {
            throw new BusinessException(DomainMessages.Catalog.SummaryMaximo);
        }

        if (draft.SeoTitle is { Length: > 60 })
        {
            throw new BusinessException(DomainMessages.Catalog.SeoTitleMaximo);
        }

        if (draft.SeoDescription is { Length: > 160 })
        {
            throw new BusinessException(DomainMessages.Catalog.SeoDescriptionMaximo);
        }

        if (draft.PriceAmount is <= 0)
        {
            throw new BusinessException(DomainMessages.Catalog.PrecoInvalido);
        }

        CategoryId = draft.CategoryId;
        if (allowSlugChange)
        {
            Slug = CatalogSlug.Normalize(draft.Slug);
        }

        Name = draft.Name.Trim();
        Status = draft.Status;
        Featured = draft.Featured;
        SortOrder = draft.SortOrder;
        Summary = draft.Summary.Trim();
        Description = draft.Description.Trim();
        PriceAmount = draft.PriceAmount;
        Currency = draft.PriceAmount is null ? null : "BRL";
        WarrantyMonths = draft.WarrantyMonths;
        SeoTitle = EmptyToNull(draft.SeoTitle);
        SeoDescription = EmptyToNull(draft.SeoDescription);
        SeoOgImage = EmptyToNull(draft.SeoOgImage);

        _images.Clear();
        for (var index = 0; index < draft.Images.Count; index++)
        {
            _images.Add(ProductImage.Create(Id, draft.Images[index], index, isCard: false));
        }

        if (draft.CardImage is not null)
        {
            _images.Add(ProductImage.Create(Id, draft.CardImage, 0, isCard: true));
        }

        _highlights.Clear();
        for (var index = 0; index < draft.Highlights.Count; index++)
        {
            _highlights.Add(ProductHighlight.Create(Id, draft.Highlights[index], index));
        }

        _specs.Clear();
        for (var index = 0; index < draft.Specs.Count; index++)
        {
            var spec = draft.Specs[index];
            _specs.Add(ProductSpec.Create(Id, spec.Label, spec.Value, spec.Group, index));
        }

        _variants.Clear();
        for (var index = 0; index < draft.Variants.Count; index++)
        {
            var variant = draft.Variants[index];
            _variants.Add(ProductVariant.Create(Id, variant.Name, variant.Type, variant.Value, variant.Swatch, variant.Image, index));
        }

        _badges.Clear();
        foreach (var badge in draft.Badges)
        {
            _badges.Add(ProductBadge.Create(Id, badge));
        }

        _related.Clear();
        for (var index = 0; index < draft.RelatedProductIds.Count; index++)
        {
            var relatedId = draft.RelatedProductIds[index];
            if (relatedId == Id)
            {
                throw new BusinessException(DomainMessages.Catalog.RelacionadoInvalido);
            }

            _related.Add(ProductRelated.Create(Id, relatedId, index));
        }

        if (Status == ProductStatuses.Active && _highlights.Count != 3)
        {
            throw new BusinessException(DomainMessages.Catalog.HighlightsObrigatorios);
        }

        if (Status == ProductStatuses.Active && !_images.Any(image => !image.IsCard))
        {
            throw new BusinessException(DomainMessages.Catalog.ImagemObrigatoria);
        }
    }

    private static string? EmptyToNull(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public sealed record ProductDraft(
    Guid CategoryId,
    string Name,
    string Slug,
    string Status,
    bool Featured,
    int SortOrder,
    string Summary,
    string Description,
    decimal? PriceAmount,
    int? WarrantyMonths,
    string? SeoTitle,
    string? SeoDescription,
    string? SeoOgImage,
    IReadOnlyList<CatalogImage> Images,
    CatalogImage? CardImage,
    IReadOnlyList<string> Highlights,
    IReadOnlyList<ProductSpecDraft> Specs,
    IReadOnlyList<ProductVariantDraft> Variants,
    IReadOnlyList<string> Badges,
    IReadOnlyList<Guid> RelatedProductIds);

public sealed record ProductSpecDraft(string Label, string Value, string? Group);

public sealed record ProductVariantDraft(
    string Name,
    string Type,
    string Value,
    string? Swatch,
    CatalogImage? Image);
