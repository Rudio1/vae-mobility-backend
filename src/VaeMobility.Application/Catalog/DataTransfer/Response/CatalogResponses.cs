namespace VaeMobility.Application.Catalog.DataTransfer.Response;

public sealed record CatalogImageResponse(string Src, string Alt, int Width, int Height);

public sealed record CatalogSeoResponse(string? Title, string? Description, string? OgImage);

public sealed record CategoryImagesResponse(
    CatalogImageResponse Explore,
    CatalogImageResponse? Menu,
    CatalogImageResponse? Showcase,
    CatalogImageResponse? Hero);

public sealed record CategoryResponse(
    Guid Id,
    string Slug,
    string Name,
    string ShortDescription,
    string Description,
    CategoryImagesResponse Images,
    int SortOrder,
    CatalogSeoResponse Seo);

public sealed record PriceResponse(decimal Amount, string Currency);

public sealed record ProductSpecResponse(string Label, string Value, string? Group);

public sealed record ProductVariantResponse(
    Guid Id,
    string Name,
    string Type,
    string Value,
    string? Swatch,
    CatalogImageResponse? Image);

public sealed record ProductResponse(
    Guid Id,
    string Slug,
    string Name,
    Guid CategoryId,
    string Status,
    bool Featured,
    int SortOrder,
    string Summary,
    string Description,
    IReadOnlyList<string> Highlights,
    IReadOnlyList<CatalogImageResponse> Images,
    CatalogImageResponse? CardImage,
    PriceResponse? Price,
    IReadOnlyList<ProductSpecResponse> Specs,
    IReadOnlyList<ProductVariantResponse> Variants,
    IReadOnlyList<string>? Badges,
    int? WarrantyMonths,
    IReadOnlyList<Guid>? RelatedProductIds,
    CatalogSeoResponse Seo,
    DateTime UpdatedAt);

public sealed record CatalogBundleResponse(
    IReadOnlyList<CategoryResponse> Categories,
    IReadOnlyList<ProductResponse> Products);
