# Catálogo

[← Voltar ao índice](./README.md)

Shape público idêntico a `vae-mobility/src/lib/catalog/types.ts`. Ver também [docs/01-catalogo-contrato.md](../docs/01-catalogo-contrato.md).

**Imagens:** `src` pode ser path `/images/...` ou URL `https://` (R2). O Zod do site ainda exige `/` — o front precisa aceitar URL ao ligar o CMS.

Enums como string: `active|draft|archived`, `desempenho|bateria|dimensoes|seguranca|geral`, `cor|tamanho|bateria`, `novo|mais-vendido|lancamento|promocao`, slots `explore|menu|showcase|hero`.

---

## Público (sem auth) — só `status = active`

### GET /api/catalog

**Finalidade:** catálogo completo para `catalogSchema` do site.

**Resposta 200 — data:** `{ "categories": Category[], "products": Product[] }`

---

### GET /api/catalog/categories

**Resposta 200 — data:** `Category[]`

---

### GET /api/catalog/categories/{slug}

**Resposta 200 — data:** `Category`  
**404** se não existir.

---

### GET /api/catalog/products

**Query:** `categorySlug?`, `featured?` (bool)

**Resposta 200 — data:** `Product[]`  
Ordenação: featured desc, sortOrder asc, name asc.

---

### GET /api/catalog/products/{slug}

**Resposta 200 — data:** `Product`  
**404** se não existir ou não for `active`.

---

## Admin (`[Authorize]`)

### GET /api/admin/categories

Lista todas (exceto soft-deleted).

### GET /api/admin/categories/{id}

### POST /api/admin/categories

**Request:** name, slug, shortDescription, description, sortOrder, seo, images (explore obrigatório).

### PUT /api/admin/categories/{id}

Slug **não** pode mudar.

### DELETE /api/admin/categories/{id}

Soft delete. Falha com `BUSINESS_ERROR` se houver produtos.

---

### GET /api/admin/products

Query: `status?`, `categoryId?`

### GET /api/admin/products/{id}

### POST /api/admin/products

Campos do `Product` (sem `id`/`updatedAt`). `status=active` dispara validação de publish.

### PUT /api/admin/products/{id}

Slug imutável se o produto já está `active`.

### DELETE /api/admin/products/{id}

Soft delete.

---

## ProductImage (request/response)

```json
{ "src": "https://...", "alt": "Motocicleta VAE A9", "width": 1254, "height": 1254 }
```

## Product (response pública)

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "slug": "vae-a9",
  "name": "VAE A9",
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "active",
  "featured": true,
  "sortOrder": 1,
  "summary": "Motocicleta elétrica urbana...",
  "description": "A VAE A9 é...",
  "highlights": ["Autonomia até 60 km", "Motor 1.200 ou 2.000 W", "Banco duplo"],
  "images": [{ "src": "https://...", "alt": "...", "width": 1254, "height": 1254 }],
  "cardImage": null,
  "price": null,
  "specs": [{ "label": "Motor", "value": "1.200 W", "group": "desempenho" }],
  "variants": [],
  "badges": ["novo"],
  "warrantyMonths": 3,
  "relatedProductIds": [],
  "seo": { "title": null, "description": "...", "ogImage": null },
  "updatedAt": "2026-09-18T20:00:00Z"
}
```

---

## POST /api/admin/media

**Autenticação:** JWT  
**Content-Type:** `multipart/form-data` (`file`)

**Resposta 200 — data:** `{ "src", "width", "height" }`

Sem config Cloudflare: `BUSINESS_ERROR` informando que R2 não está configurado.
