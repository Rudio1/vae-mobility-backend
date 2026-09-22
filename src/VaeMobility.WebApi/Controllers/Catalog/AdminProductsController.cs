using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaeMobility.Application.Catalog.DataTransfer.Request;
using VaeMobility.Application.Catalog.Services.Interfaces;
using VaeMobility.WebApi.Controllers.Generic;

namespace VaeMobility.WebApi.Controllers.Catalog;

[Route("api/admin/products")]
[Authorize]
public sealed class AdminProductsController(ICatalogAppService catalogAppService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? status, [FromQuery] Guid? categoryId, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.ListAdminProductsAsync(status, categoryId, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.GetAdminProductAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaveProductRequest request, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.CreateProductAsync(request, cancellationToken));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SaveProductRequest request, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.UpdateProductAsync(id, request, cancellationToken));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.DeleteProductAsync(id, cancellationToken));
}
