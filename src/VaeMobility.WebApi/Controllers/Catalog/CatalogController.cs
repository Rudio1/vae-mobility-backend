using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaeMobility.Application.Catalog.Services.Interfaces;
using VaeMobility.WebApi.Controllers.Generic;

namespace VaeMobility.WebApi.Controllers.Catalog;

[Route("api/catalog")]
[AllowAnonymous]
public sealed class CatalogController(ICatalogAppService catalogAppService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCatalog(CancellationToken cancellationToken)
        => FromResult(await catalogAppService.GetPublicCatalogAsync(cancellationToken));

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        => FromResult(await catalogAppService.GetPublicCategoriesAsync(cancellationToken));

    [HttpGet("categories/{slug}")]
    public async Task<IActionResult> GetCategory(string slug, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.GetPublicCategoryBySlugAsync(slug, cancellationToken));

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? categorySlug,
        [FromQuery] bool? featured,
        CancellationToken cancellationToken)
        => FromResult(await catalogAppService.GetPublicProductsAsync(categorySlug, featured, cancellationToken));

    [HttpGet("products/{slug}")]
    public async Task<IActionResult> GetProduct(string slug, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.GetPublicProductBySlugAsync(slug, cancellationToken));
}
