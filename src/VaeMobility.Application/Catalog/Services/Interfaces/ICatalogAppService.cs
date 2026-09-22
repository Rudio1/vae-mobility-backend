using VaeMobility.Application.Catalog.DataTransfer.Request;
using VaeMobility.Application.Catalog.DataTransfer.Response;
using VaeMobility.Application.Generic;

namespace VaeMobility.Application.Catalog.Services.Interfaces;

public interface ICatalogAppService
{
    Task<Result<CatalogBundleResponse>> GetPublicCatalogAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<CategoryResponse>>> GetPublicCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> GetPublicCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProductResponse>>> GetPublicProductsAsync(string? categorySlug, bool? featured, CancellationToken cancellationToken = default);
    Task<Result<ProductResponse>> GetPublicProductBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<CategoryResponse>>> ListAdminCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> GetAdminCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> CreateCategoryAsync(SaveCategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> UpdateCategoryAsync(Guid id, SaveCategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ProductResponse>>> ListAdminProductsAsync(string? status, Guid? categoryId, CancellationToken cancellationToken = default);
    Task<Result<ProductResponse>> GetAdminProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ProductResponse>> CreateProductAsync(SaveProductRequest request, CancellationToken cancellationToken = default);
    Task<Result<ProductResponse>> UpdateProductAsync(Guid id, SaveProductRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
}
