using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaeMobility.Application.Catalog.DataTransfer.Request;
using VaeMobility.Application.Catalog.Services.Interfaces;
using VaeMobility.WebApi.Controllers.Generic;

namespace VaeMobility.WebApi.Controllers.Catalog;

[Route("api/admin/categories")]
[Authorize]
public sealed class AdminCategoriesController(ICatalogAppService catalogAppService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
        => FromResult(await catalogAppService.ListAdminCategoriesAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.GetAdminCategoryAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SaveCategoryRequest request, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.CreateCategoryAsync(request, cancellationToken));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SaveCategoryRequest request, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.UpdateCategoryAsync(id, request, cancellationToken));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => FromResult(await catalogAppService.DeleteCategoryAsync(id, cancellationToken));
}
