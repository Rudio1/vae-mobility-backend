using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaeMobility.Application.Media.Commands.UploadMedia;
using VaeMobility.WebApi.Controllers.Generic;

namespace VaeMobility.WebApi.Controllers.Catalog;

[Route("api/admin/media")]
[Authorize]
public sealed class AdminMediaController(IUploadMediaHandler uploadMediaHandler) : ApiControllerBase
{
    [HttpPost]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> Upload(IFormFile? file, [FromForm] int? width, [FromForm] int? height, CancellationToken cancellationToken)
    {
        if (file is null)
        {
            return FromResult(Application.Generic.Result<MediaResponse>.Fail(
                Domain.Generic.Validations.DomainMessages.Media.ArquivoObrigatorio));
        }

        await using var stream = file.OpenReadStream();
        var result = await uploadMediaHandler.HandleAsync(
            new UploadMediaCommand(stream, file.FileName, file.ContentType, width, height),
            cancellationToken);
        return FromResult(result);
    }
}
