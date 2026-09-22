using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Domain.Catalog.Enums;
using VaeMobility.Domain.Catalog.Repositories.Interfaces;
using VaeMobility.Domain.Catalog.Services.Interfaces;
using VaeMobility.Domain.Catalog.ValueObjects;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Domain.Catalog.Services;

public sealed class CatalogDomainService(
    ICategoryRepository categoryRepository,
    IProductRepository productRepository) : ICatalogDomainService
{
    public async Task<Category> CreateCategoryAsync(
        string name,
        string slug,
        string shortDescription,
        string description,
        int sortOrder,
        string? seoTitle,
        string? seoDescription,
        string? seoOgImage,
        IReadOnlyDictionary<string, CatalogImage> images,
        CancellationToken cancellationToken = default)
    {
        var normalized = CatalogSlug.Normalize(slug);
        if (await categoryRepository.SlugExistsAsync(normalized, null, cancellationToken))
        {
            throw new BusinessException(DomainMessages.Catalog.SlugDuplicado);
        }

        var category = Category.Create(name, normalized, shortDescription, description, sortOrder, seoTitle, seoDescription, seoOgImage, images);
        await categoryRepository.AddAsync(category, cancellationToken);
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(
        Guid id,
        string name,
        string shortDescription,
        string description,
        int sortOrder,
        string? seoTitle,
        string? seoDescription,
        string? seoOgImage,
        IReadOnlyDictionary<string, CatalogImage> images,
        CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(DomainMessages.Catalog.CategoriaNaoEncontrada);

        category.Update(name, shortDescription, description, sortOrder, seoTitle, seoDescription, seoOgImage, images);
        categoryRepository.Update(category);
        return category;
    }

    public async Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(DomainMessages.Catalog.CategoriaNaoEncontrada);

        if (await categoryRepository.HasProductsAsync(id, cancellationToken))
        {
            throw new BusinessException(DomainMessages.Catalog.CategoriaComProdutos);
        }

        categoryRepository.Remove(category);
    }

    public Task<Category?> GetCategoryAsync(Guid id, CancellationToken cancellationToken = default)
        => categoryRepository.GetByIdAsync(id, cancellationToken);

    public Task<Category?> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => categoryRepository.GetBySlugAsync(slug, cancellationToken);

    public Task<IReadOnlyList<Category>> ListCategoriesAsync(CancellationToken cancellationToken = default)
        => categoryRepository.ListAsync(cancellationToken);

    public async Task<Product> CreateProductAsync(ProductDraft draft, CancellationToken cancellationToken = default)
    {
        await EnsureProductRulesAsync(draft, null, cancellationToken);
        var product = Product.Create(draft);
        await productRepository.AddAsync(product, cancellationToken);
        return product;
    }

    public async Task<Product> UpdateProductAsync(Guid id, ProductDraft draft, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(DomainMessages.Catalog.ProdutoNaoEncontrado);

        await EnsureProductRulesAsync(draft, id, cancellationToken);
        product.Update(draft);
        productRepository.Update(product);
        return product;
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(DomainMessages.Catalog.ProdutoNaoEncontrado);

        productRepository.Remove(product);
    }

    public Task<Product?> GetProductAsync(Guid id, CancellationToken cancellationToken = default)
        => productRepository.GetByIdAsync(id, cancellationToken);

    public Task<Product?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => productRepository.GetBySlugAsync(slug, cancellationToken);

    public Task<IReadOnlyList<Product>> ListProductsAsync(string? status, Guid? categoryId, CancellationToken cancellationToken = default)
        => productRepository.ListAsync(status, categoryId, cancellationToken);

    public Task<IReadOnlyList<Product>> ListActiveProductsAsync(string? categorySlug, bool? featured, CancellationToken cancellationToken = default)
        => productRepository.ListActiveAsync(categorySlug, featured, cancellationToken);

    private async Task EnsureProductRulesAsync(ProductDraft draft, Guid? excludingId, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(draft.CategoryId, cancellationToken)
            ?? throw new NotFoundException(DomainMessages.Catalog.CategoriaNaoEncontrada);

        if (await productRepository.SlugExistsAsync(CatalogSlug.Normalize(draft.Slug), excludingId, cancellationToken))
        {
            throw new BusinessException(DomainMessages.Catalog.SlugDuplicado);
        }

        if (draft.RelatedProductIds.Count > 0 &&
            !await productRepository.AllExistAsync(draft.RelatedProductIds, cancellationToken))
        {
            throw new BusinessException(DomainMessages.Catalog.RelacionadoInvalido);
        }

        if (draft.Status == ProductStatuses.Active && !category.HasExplore)
        {
            throw new BusinessException(DomainMessages.Catalog.CategoriaSemExplore);
        }
    }
}
