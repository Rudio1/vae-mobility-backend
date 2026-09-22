using VaeMobility.Application.Generic;
using VaeMobility.Application.Media.Services.Interfaces;

namespace VaeMobility.Application.Media.Commands.UploadMedia;

public sealed record UploadMediaCommand(Stream Content, string FileName, string ContentType, int? Width, int? Height);

public sealed record MediaResponse(string Src, int Width, int Height);

public interface IUploadMediaHandler
{
    Task<Result<MediaResponse>> HandleAsync(UploadMediaCommand command, CancellationToken cancellationToken = default);
}

public sealed class UploadMediaHandler(IMediaStorage mediaStorage) : IUploadMediaHandler
{
    private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/webp", "image/gif"
    };

    public async Task<Result<MediaResponse>> HandleAsync(UploadMediaCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Content.Length == 0 || string.IsNullOrWhiteSpace(command.FileName))
        {
            return Result<MediaResponse>.Fail(Domain.Generic.Validations.DomainMessages.Media.ArquivoObrigatorio);
        }

        if (!AllowedTypes.Contains(command.ContentType))
        {
            return Result<MediaResponse>.Fail(Domain.Generic.Validations.DomainMessages.Media.TipoInvalido);
        }

        if (!mediaStorage.IsConfigured)
        {
            return Result<MediaResponse>.Fail(Domain.Generic.Validations.DomainMessages.Media.R2NaoConfigurado);
        }

        var stored = await mediaStorage.UploadAsync(
            new MediaUpload(command.Content, command.FileName, command.ContentType),
            command.Width,
            command.Height,
            cancellationToken);

        return Result<MediaResponse>.Ok(new MediaResponse(stored.Src, stored.Width, stored.Height));
    }
}
