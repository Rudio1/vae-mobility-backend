using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Domain.Catalog.ValueObjects;

namespace VaeMobility.Domain.Catalog.Services.Interfaces;

public interface ICatalogDomainService
{
    Task<Category> CreateCategoryAsync(
        string name,
        string slug,
        string shortDescription,
        string description,
        int sortOrder,
        string? seoTitle,
        string? seoDescription,
        string? seoOgImage,
        IReadOnlyDictionary<string, CatalogImage> images,
        CancellationToken cancellationToken = default);

    Task<Category> UpdateCategoryAsync(
        Guid id,
        string name,
        string shortDescription,
        string description,
        int sortOrder,
        string? seoTitle,
        string? seoDescription,
        string? seoOgImage,
        IReadOnlyDictionary<string, CatalogImage> images,
        CancellationToken cancellationToken = default);

    Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Category>> ListCategoriesAsync(CancellationToken cancellationToken = default);

    Task<Product> CreateProductAsync(ProductDraft draft, CancellationToken cancellationToken = default);
    Task<Product> UpdateProductAsync(Guid id, ProductDraft draft, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> GetProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ListProductsAsync(string? status, Guid? categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ListActiveProductsAsync(string? categorySlug, bool? featured, CancellationToken cancellationToken = default);
}
