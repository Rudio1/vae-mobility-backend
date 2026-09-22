using System.Text.RegularExpressions;
using VaeMobility.Domain.Generic.Validations;

namespace VaeMobility.Domain.Catalog.ValueObjects;

public static partial class CatalogSlug
{
    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.CultureInvariant)]
    private static partial Regex SlugRegex();

    public static string Normalize(string slug)
    {
        var value = slug.Trim().ToLowerInvariant();
        if (!SlugRegex().IsMatch(value))
        {
            throw new BusinessException(DomainMessages.Catalog.SlugInvalido);
        }

        return value;
    }
}
