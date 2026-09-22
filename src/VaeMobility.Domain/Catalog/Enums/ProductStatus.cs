namespace VaeMobility.Domain.Catalog.Enums;

public static class ProductStatuses
{
    public const string Active = "active";
    public const string Draft = "draft";
    public const string Archived = "archived";

    public static readonly string[] All = [Active, Draft, Archived];
}

public static class SpecGroups
{
    public const string Desempenho = "desempenho";
    public const string Bateria = "bateria";
    public const string Dimensoes = "dimensoes";
    public const string Seguranca = "seguranca";
    public const string Geral = "geral";
}

public static class VariantTypes
{
    public const string Cor = "cor";
    public const string Tamanho = "tamanho";
    public const string Bateria = "bateria";
}

public static class ProductBadges
{
    public const string Novo = "novo";
    public const string MaisVendido = "mais-vendido";
    public const string Lancamento = "lancamento";
    public const string Promocao = "promocao";
}

public static class CategoryImageSlots
{
    public const string Explore = "explore";
    public const string Menu = "menu";
    public const string Showcase = "showcase";
    public const string Hero = "hero";
}
