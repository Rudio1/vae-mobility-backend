namespace VaeMobility.Application.Media.Services.Interfaces;

public sealed record MediaUpload(Stream Content, string FileName, string ContentType);

public sealed record StoredMedia(string Src, int Width, int Height);

public interface IMediaStorage
{
    bool IsConfigured { get; }
    Task<StoredMedia> UploadAsync(MediaUpload upload, int? width, int? height, CancellationToken cancellationToken = default);
}
