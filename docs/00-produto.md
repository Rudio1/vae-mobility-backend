# 00 — Produto VAE Mobility

Este backend é o **CMS do site** [vae-mobility](C:\Users\Guilherme\Documents\VaeMobility\vae-mobility).

## O que o site é

Catálogo institucional de veículos elétricos + **funil de vendas no WhatsApp**.

- Sem checkout, sem carrinho, sem pagamento
- O visitante escolhe um modelo e fala com a empresa no WhatsApp
- Dados do catálogo no site hoje estão em TypeScript (`src/data/*`); a Fase 3 do front troca isso por `CmsCatalogRepository` consumindo esta API

## O que este CMS cobre

| Contexto | Responsabilidade |
|---|---|
| `Auth` | Login de um admin (JWT em cookie HttpOnly) |
| `Catalog` | Categorias, produtos, descrições, specs, variantes, badges, relacionados |
| `Media` | Upload de imagens para Cloudflare R2 |

## O que NÃO cobre (ainda)

- Hero, FAQ, página Sobre, política de privacidade
- Número/templates de WhatsApp e dados da empresa
- Leads (o formulário do site abre o WhatsApp; nada é gravado aqui)
- Painel admin web (use Swagger)
- Multi-tenant, Identity completo, jobs, migrations EF

## Regras de negócio do catálogo

- `price: null` = "preço sob consulta". Nunca gravar `0`.
- `highlights`: exatamente 3 para publicar `status = active`.
- Categoria: slot de imagem `explore` obrigatório para existir no catálogo público.
- Slug kebab-case (`vae-a9`), único por tipo, **imutável** depois que o produto fica `active` (categoria: imutável após criação).
- API pública (`/api/catalog`) devolve só produtos `active`.
- IDs são `Guid` serializados como string (o front aceita `id: string`).

## Contrato com o front

Fonte da verdade dos tipos: `vae-mobility/src/lib/catalog/types.ts`.

Mapeamento campo a campo: [01-catalogo-contrato.md](./01-catalogo-contrato.md).

Rotas: [api-contract/catalog.md](../api-contract/catalog.md) e [api-contract/auth.md](../api-contract/auth.md).

**Divergência conhecida:** o Zod do site hoje exige `image.src` começando com `/`. Com R2 o `src` será `https://...`. O front precisa relaxar o schema quando ligar o CMS.

## Schema do banco

Não usamos EF Migrations. Scripts manuais em [sql/](../sql/). Leia [sql/README.md](../sql/README.md).
