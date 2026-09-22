namespace VaeMobility.Domain.Generic.Validations;

public static class DomainMessages
{
    public static class Auth
    {
        public const string UsuarioNaoEncontrado = "Usuario nao encontrado.";
        public const string CredenciaisInvalidas = "Email ou senha invalidos.";
        public const string RefreshTokenInvalido = "Sessao expirada. Faca login novamente.";
        public const string UsuarioInativo = "Usuario inativo nao pode autenticar.";
    }

    public static class Catalog
    {
        public const string SlugInvalido = "Slug deve ser minusculo, sem acento, com hifens.";
        public const string SlugDuplicado = "Ja existe um registro com este slug.";
        public const string SlugImutavel = "Slug nao pode ser alterado.";
        public const string CategoriaNaoEncontrada = "Categoria nao encontrada.";
        public const string ProdutoNaoEncontrado = "Produto nao encontrado.";
        public const string CategoriaComProdutos = "Nao e possivel excluir categoria com produtos.";
        public const string ExploreObrigatorio = "A categoria precisa da imagem explore.";
        public const string HighlightsObrigatorios = "Produto ativo precisa de exatamente 3 destaques.";
        public const string ImagemObrigatoria = "Produto ativo precisa de pelo menos uma imagem.";
        public const string PrecoInvalido = "Preco deve ser nulo (sob consulta) ou maior que zero.";
        public const string RelacionadoInvalido = "Produto relacionado inexistente ou auto-referencia.";
        public const string CategoriaSemExplore = "Categoria do produto precisa da imagem explore.";
        public const string SummaryMaximo = "Resumo deve ter no maximo 160 caracteres.";
        public const string SeoTitleMaximo = "SEO title deve ter no maximo 60 caracteres.";
        public const string SeoDescriptionMaximo = "SEO description deve ter no maximo 160 caracteres.";
        public const string ImagemInvalida = "Imagem precisa de src, alt, width e height.";
    }

    public static class Media
    {
        public const string ArquivoObrigatorio = "Arquivo de imagem e obrigatorio.";
        public const string TipoInvalido = "Envie uma imagem (jpeg, png, webp, gif).";
        public const string R2NaoConfigurado = "Cloudflare R2 nao esta configurado.";
        public const string UploadFalhou = "Falha ao enviar a imagem.";
    }
}
