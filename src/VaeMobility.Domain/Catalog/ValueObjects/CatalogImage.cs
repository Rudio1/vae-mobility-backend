using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Domain.Catalog.ValueObjects;

public sealed record CatalogImage(string Src, string Alt, int Width, int Height)
{
    public static CatalogImage Create(string src, string alt, int width, int height)
    {
        if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(alt) || width <= 0 || height <= 0)
        {
            throw new BusinessException(DomainMessages.Catalog.ImagemInvalida);
        }

        return new CatalogImage(src.Trim(), alt.Trim(), width, height);
    }
}
