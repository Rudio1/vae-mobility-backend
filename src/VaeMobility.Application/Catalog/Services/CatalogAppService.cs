using FluentValidation;
using VaeMobility.Application.Catalog.DataTransfer.Request;
using VaeMobility.Application.Catalog.DataTransfer.Response;
using VaeMobility.Application.Catalog.Profiles;
using VaeMobility.Application.Catalog.Services.Interfaces;
using VaeMobility.Application.Generic;
using VaeMobility.Domain.Catalog.Enums;
using VaeMobility.Domain.Catalog.Services.Interfaces;
using VaeMobility.Domain.Generic.Repositories.Interfaces;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Application.Catalog.Services;

public sealed class CatalogAppService(
    ICatalogDomainService catalogDomainService,
    IUnitOfWork unitOfWork,
    IValidator<SaveCategoryRequest> categoryValidator,
    IValidator<SaveProductRequest> productValidator) : ICatalogAppService
{
    public async Task<Result<CatalogBundleResponse>> GetPublicCatalogAsync(CancellationToken cancellationToken = default)
    {
        var categories = await catalogDomainService.ListCategoriesAsync(cancellationToken);
        var products = await catalogDomainService.ListActiveProductsAsync(null, null, cancellationToken);
        return Result<CatalogBundleResponse>.Ok(new CatalogBundleResponse(
            categories.Select(item => item.ToResponse()).ToArray(),
            products.Select(item => item.ToResponse()).ToArray()));
    }

    public async Task<Result<IReadOnlyList<CategoryResponse>>> GetPublicCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await catalogDomainService.ListCategoriesAsync(cancellationToken);
        return Result<IReadOnlyList<CategoryResponse>>.Ok(categories.Select(item => item.ToResponse()).ToArray());
    }

    public async Task<Result<CategoryResponse>> GetPublicCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var category = await catalogDomainService.GetCategoryBySlugAsync(slug, cancellationToken);
        return category is null
            ? Result<CategoryResponse>.Fail(DomainMessages.Catalog.CategoriaNaoEncontrada)
            : Result<CategoryResponse>.Ok(category.ToResponse());
    }

    public async Task<Result<IReadOnlyList<ProductResponse>>> GetPublicProductsAsync(
        string? categorySlug,
        bool? featured,
        CancellationToken cancellationToken = default)
    {
        var products = await catalogDomainService.ListActiveProductsAsync(categorySlug, featured, cancellationToken);
        return Result<IReadOnlyList<ProductResponse>>.Ok(products.Select(item => item.ToResponse()).ToArray());
    }

    public async Task<Result<ProductResponse>> GetPublicProductBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var product = await catalogDomainService.GetProductBySlugAsync(slug, cancellationToken);
        if (product is null || product.Status != ProductStatuses.Active)
        {
            return Result<ProductResponse>.Fail(DomainMessages.Catalog.ProdutoNaoEncontrado);
        }

        return Result<ProductResponse>.Ok(product.ToResponse());
    }

    public async Task<Result<IReadOnlyList<CategoryResponse>>> ListAdminCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await catalogDomainService.ListCategoriesAsync(cancellationToken);
        return Result<IReadOnlyList<CategoryResponse>>.Ok(categories.Select(item => item.ToResponse()).ToArray());
    }

    public async Task<Result<CategoryResponse>> GetAdminCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await catalogDomainService.GetCategoryAsync(id, cancellationToken);
        return category is null
            ? Result<CategoryResponse>.Fail(DomainMessages.Catalog.CategoriaNaoEncontrada)
            : Result<CategoryResponse>.Ok(category.ToResponse());
    }

    public async Task<Result<CategoryResponse>> CreateCategoryAsync(SaveCategoryRequest request, CancellationToken cancellationToken = default)
    {
        await categoryValidator.ValidateAndThrowAsync(request, cancellationToken);
        var category = await catalogDomainService.CreateCategoryAsync(
            request.Name,
            request.Slug,
            request.ShortDescription,
            request.Description,
            request.SortOrder,
            request.Seo?.Title,
            request.Seo?.Description,
            request.Seo?.OgImage,
            request.Images.ToImageMap(),
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<CategoryResponse>.Ok(category.ToResponse());
    }

    public async Task<Result<CategoryResponse>> UpdateCategoryAsync(Guid id, SaveCategoryRequest request, CancellationToken cancellationToken = default)
    {
        await categoryValidator.ValidateAndThrowAsync(request, cancellationToken);
        var category = await catalogDomainService.UpdateCategoryAsync(
            id,
            request.Name,
            request.ShortDescription,
            request.Description,
            request.SortOrder,
            request.Seo?.Title,
            request.Seo?.Description,
            request.Seo?.OgImage,
            request.Images.ToImageMap(),
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<CategoryResponse>.Ok(category.ToResponse());
    }

    public async Task<Result> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await catalogDomainService.DeleteCategoryAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<IReadOnlyList<ProductResponse>>> ListAdminProductsAsync(
        string? status,
        Guid? categoryId,
        CancellationToken cancellationToken = default)
    {
        var products = await catalogDomainService.ListProductsAsync(status, categoryId, cancellationToken);
        return Result<IReadOnlyList<ProductResponse>>.Ok(products.Select(item => item.ToResponse()).ToArray());
    }

    public async Task<Result<ProductResponse>> GetAdminProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await catalogDomainService.GetProductAsync(id, cancellationToken);
        return product is null
            ? Result<ProductResponse>.Fail(DomainMessages.Catalog.ProdutoNaoEncontrado)
            : Result<ProductResponse>.Ok(product.ToResponse());
    }

    public async Task<Result<ProductResponse>> CreateProductAsync(SaveProductRequest request, CancellationToken cancellationToken = default)
    {
        await productValidator.ValidateAndThrowAsync(request, cancellationToken);
        var product = await catalogDomainService.CreateProductAsync(request.ToDraft(), cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ProductResponse>.Ok(product.ToResponse());
    }

    public async Task<Result<ProductResponse>> UpdateProductAsync(Guid id, SaveProductRequest request, CancellationToken cancellationToken = default)
    {
        await productValidator.ValidateAndThrowAsync(request, cancellationToken);
        var product = await catalogDomainService.UpdateProductAsync(id, request.ToDraft(), cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ProductResponse>.Ok(product.ToResponse());
    }

    public async Task<Result> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await catalogDomainService.DeleteProductAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}
