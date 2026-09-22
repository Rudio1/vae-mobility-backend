namespace VaeMobility.Application.Catalog.DataTransfer.Request;

public sealed record CatalogImageRequest(string Src, string Alt, int Width, int Height);

public sealed record CatalogSeoRequest(string? Title, string? Description, string? OgImage);

public sealed record CategoryImagesRequest(
    CatalogImageRequest Explore,
    CatalogImageRequest? Menu,
    CatalogImageRequest? Showcase,
    CatalogImageRequest? Hero);

public sealed record SaveCategoryRequest(
    string Name,
    string Slug,
    string ShortDescription,
    string Description,
    int SortOrder,
    CatalogSeoRequest? Seo,
    CategoryImagesRequest Images);

public sealed record ProductSpecRequest(string Label, string Value, string? Group);

public sealed record ProductVariantRequest(
    string Name,
    string Type,
    string Value,
    string? Swatch,
    CatalogImageRequest? Image);

public sealed record PriceRequest(decimal Amount, string Currency);

public sealed record SaveProductRequest(
    Guid CategoryId,
    string Name,
    string Slug,
    string Status,
    bool Featured,
    int SortOrder,
    string Summary,
    string Description,
    PriceRequest? Price,
    int? WarrantyMonths,
    CatalogSeoRequest? Seo,
    IReadOnlyList<CatalogImageRequest> Images,
    CatalogImageRequest? CardImage,
    IReadOnlyList<string> Highlights,
    IReadOnlyList<ProductSpecRequest> Specs,
    IReadOnlyList<ProductVariantRequest> Variants,
    IReadOnlyList<string>? Badges,
    IReadOnlyList<Guid>? RelatedProductIds);
