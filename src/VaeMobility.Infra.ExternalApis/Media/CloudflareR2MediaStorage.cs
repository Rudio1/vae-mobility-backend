using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using VaeMobility.Application.Media.Services.Interfaces;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Infra.ExternalApis.Media;

public sealed class CloudflareR2MediaStorage(IOptions<CloudflareR2Options> options) : IMediaStorage
{
    public bool IsConfigured
    {
        get
        {
            var value = options.Value;
            return !string.IsNullOrWhiteSpace(value.AccountId)
                && !string.IsNullOrWhiteSpace(value.AccessKey)
                && !string.IsNullOrWhiteSpace(value.SecretKey)
                && !string.IsNullOrWhiteSpace(value.Bucket)
                && !string.IsNullOrWhiteSpace(value.PublicBaseUrl);
        }
    }

    public async Task<StoredMedia> UploadAsync(
        MediaUpload upload,
        int? width,
        int? height,
        CancellationToken cancellationToken = default)
    {
        if (!IsConfigured)
        {
            throw new BusinessException(DomainMessages.Media.R2NaoConfigurado);
        }

        var key = $"catalog/{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid():N}{Path.GetExtension(upload.FileName)}";
        var config = options.Value;

        var serviceUrl = string.IsNullOrWhiteSpace(config.ServiceUrl)
            ? $"https://{config.AccountId}.r2.cloudflarestorage.com"
            : config.ServiceUrl.TrimEnd('/');

        using var client = new AmazonS3Client(
            config.AccessKey,
            config.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true
            });

        var request = new PutObjectRequest
        {
            BucketName = config.Bucket,
            Key = key,
            InputStream = upload.Content,
            ContentType = upload.ContentType,
            DisablePayloadSigning = true
        };

        try
        {
            await client.PutObjectAsync(request, cancellationToken);
        }
        catch (AmazonS3Exception)
        {
            throw new BusinessException(DomainMessages.Media.UploadFalhou);
        }

        var baseUrl = config.PublicBaseUrl.TrimEnd('/');
        return new StoredMedia($"{baseUrl}/{key}", width.GetValueOrDefault(1), height.GetValueOrDefault(1));
    }
}
