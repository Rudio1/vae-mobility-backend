using VaeMobility.Application.Catalog.DataTransfer.Request;
using VaeMobility.Application.Catalog.DataTransfer.Response;
using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Domain.Catalog.Enums;
using VaeMobility.Domain.Catalog.ValueObjects;

namespace VaeMobility.Application.Catalog.Profiles;

public static class CatalogMapper
{
    public static CatalogImage ToImage(this CatalogImageRequest request)
        => CatalogImage.Create(request.Src, request.Alt, request.Width, request.Height);

    public static Dictionary<string, CatalogImage> ToImageMap(this CategoryImagesRequest request)
    {
        var images = new Dictionary<string, CatalogImage>
        {
            [CategoryImageSlots.Explore] = request.Explore.ToImage()
        };

        if (request.Menu is not null) images[CategoryImageSlots.Menu] = request.Menu.ToImage();
        if (request.Showcase is not null) images[CategoryImageSlots.Showcase] = request.Showcase.ToImage();
        if (request.Hero is not null) images[CategoryImageSlots.Hero] = request.Hero.ToImage();
        return images;
    }

    public static ProductDraft ToDraft(this SaveProductRequest request)
        => new(
            request.CategoryId,
            request.Name,
            request.Slug,
            request.Status,
            request.Featured,
            request.SortOrder,
            request.Summary,
            request.Description,
            request.Price?.Amount,
            request.WarrantyMonths,
            request.Seo?.Title,
            request.Seo?.Description,
            request.Seo?.OgImage,
            request.Images.Select(image => image.ToImage()).ToArray(),
            request.CardImage?.ToImage(),
            request.Highlights,
            request.Specs.Select(spec => new ProductSpecDraft(spec.Label, spec.Value, spec.Group)).ToArray(),
            request.Variants.Select(variant => new ProductVariantDraft(
                variant.Name,
                variant.Type,
                variant.Value,
                variant.Swatch,
                variant.Image?.ToImage())).ToArray(),
            request.Badges ?? [],
            request.RelatedProductIds ?? []);

    public static CategoryResponse ToResponse(this Category category)
    {
        var explore = category.GetImage(CategoryImageSlots.Explore)
            ?? throw new InvalidOperationException("Category explore image missing.");

        return new CategoryResponse(
            category.Id,
            category.Slug,
            category.Name,
            category.ShortDescription,
            category.Description,
            new CategoryImagesResponse(
                explore.ToResponse(),
                category.GetImage(CategoryImageSlots.Menu)?.ToResponse(),
                category.GetImage(CategoryImageSlots.Showcase)?.ToResponse(),
                category.GetImage(CategoryImageSlots.Hero)?.ToResponse()),
            category.SortOrder,
            new CatalogSeoResponse(category.SeoTitle, category.SeoDescription, category.SeoOgImage));
    }

    public static ProductResponse ToResponse(this Product product)
    {
        var badges = product.Badges.Select(badge => badge.Badge).ToArray();
        var related = product.Related.OrderBy(item => item.SortOrder).Select(item => item.RelatedProductId).ToArray();

        return new ProductResponse(
            product.Id,
            product.Slug,
            product.Name,
            product.CategoryId,
            product.Status,
            product.Featured,
            product.SortOrder,
            product.Summary,
            product.Description,
            product.Highlights.OrderBy(item => item.SortOrder).Select(item => item.Text).ToArray(),
            product.GalleryImages().Select(image => image.ToResponse()).ToArray(),
            product.CardImage()?.ToResponse(),
            product.PriceAmount is null ? null : new PriceResponse(product.PriceAmount.Value, product.Currency ?? "BRL"),
            product.Specs.OrderBy(item => item.SortOrder).Select(item => new ProductSpecResponse(item.Label, item.Value, item.Group)).ToArray(),
            product.Variants.OrderBy(item => item.SortOrder).Select(item => new ProductVariantResponse(
                item.Id,
                item.Name,
                item.Type,
                item.Value,
                item.Swatch,
                item.Image?.ToResponse())).ToArray(),
            badges.Length == 0 ? null : badges,
            product.WarrantyMonths,
            related.Length == 0 ? null : related,
            new CatalogSeoResponse(product.SeoTitle, product.SeoDescription, product.SeoOgImage),
            product.UpdatedAt ?? product.CreatedAt);
    }

    private static CatalogImageResponse ToResponse(this CatalogImage image)
        => new(image.Src, image.Alt, image.Width, image.Height);
}
