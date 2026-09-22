# 01 — Contrato do catálogo (front ↔ backend)

Tipos canônicos do site: `vae-mobility/src/lib/catalog/types.ts` e `docs/07-catalogo-modelo-dados.md`.

A API pública (`GET /api/catalog*`) devolve **o mesmo shape** desses tipos (camelCase, enums string).

## Categoria

| Campo front | Tipo | Tabela / coluna | Notas |
|---|---|---|---|
| `id` | string (Guid) | `categories.id` | |
| `slug` | string | `categories.slug` | único, kebab-case, imutável após create |
| `name` | string | `categories.name` | |
| `shortDescription` | string | `categories.short_description` | |
| `description` | string | `categories.description` | parágrafos (`\n\n`) |
| `sortOrder` | int | `categories.sort_order` | |
| `seo.title` | string? | `categories.seo_title` | max 60 |
| `seo.description` | string? | `categories.seo_description` | max 160 |
| `seo.ogImage` | string? | `categories.seo_og_image` | URL |
| `images.explore` | ProductImage | `category_images` slot=`explore` | **obrigatório** |
| `images.menu` | ProductImage? | slot=`menu` | 4:3 |
| `images.showcase` | ProductImage? | slot=`showcase` | 16:10 |
| `images.hero` | ProductImage? | slot=`hero` | 4:3 |

Slots **não** compartilham a mesma foto. Sem fallback entre slots.

## Produto

| Campo front | Tipo | Tabela / coluna | Notas |
|---|---|---|---|
| `id` | string (Guid) | `products.id` | |
| `slug` | string | `products.slug` | único; imutável se `active` |
| `name` | string | `products.name` | |
| `categoryId` | string (Guid) | `products.category_id` | FK |
| `status` | `active` \| `draft` \| `archived` | `products.status` | público só `active` |
| `featured` | bool | `products.featured` | |
| `sortOrder` | int | `products.sort_order` | |
| `summary` | string | `products.summary` | max 160 |
| `description` | string | `products.description` | |
| `highlights` | string[3] | `product_highlights` | exatamente 3 para `active` |
| `images` | ProductImage[] | `product_images` | min 1 para `active`; `[0]` = principal |
| `cardImage` | ProductImage? | `product_images.is_card = 1` | 4:3 |
| `price` | `{ amount, currency:"BRL" } \| null` | `products.price_amount`, `products.currency` | `null` = consulte |
| `specs` | `{ label, value, group? }[]` | `product_specs` | group: desempenho/bateria/dimensoes/seguranca/geral |
| `variants` | Variant[] | `product_variants` | type: cor/tamanho/bateria |
| `badges` | badge[]? | `product_badges` | novo/mais-vendido/lancamento/promocao |
| `warrantyMonths` | int? | `products.warranty_months` | |
| `relatedProductIds` | string[]? | `product_related` | não pode ser o próprio |
| `seo.*` | CatalogSeo | `products.seo_*` | |
| `updatedAt` | ISO 8601 | `products.updated_at` | sitemap |

## ProductImage

| Campo | Tipo | Notas |
|---|---|---|
| `src` | string | URL R2 (`https://...`) ou path `/images/...` |
| `alt` | string | obrigatório |
| `width` | int > 0 | |
| `height` | int > 0 | |

O Zod do site ainda exige `src` começando com `/`. Com CMS/R2 isso muda para URL absoluta.

## Variant

| Campo | Tipo |
|---|---|
| `id` | Guid string |
| `name` | string |
| `type` | `cor` \| `tamanho` \| `bateria` |
| `value` | string |
| `swatch` | hex? |
| `image` | ProductImage? |

## Ordenação pública

`featured` desc, `sortOrder` asc, `name` asc.

## Validação ao publicar (`status = active`)

- Slug válido e único
- Exatamente 3 highlights
- Pelo menos 1 imagem de galeria
- Categoria existente e não deletada
- Categoria com slot `explore`
- `relatedProductIds` existentes e diferentes do próprio
- `price.amount` > 0 se price não for null

## Endpoints

Ver [api-contract/catalog.md](../api-contract/catalog.md).
