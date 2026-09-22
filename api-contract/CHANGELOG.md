# Changelog — API Contract (VaeMobility)

Registro de alterações no contrato da API em `api-contract/`.

Sempre que criar, alterar ou remover rota, DTO, enum ou comportamento documentado, **adicione uma entrada no topo**.

---

## [2026-09-18] — Auth + catálogo CMS

**Tipo:** Criação

**Motivo:**
CMS do site: login admin e contrato do catálogo alinhado ao Zod do front.

**Depois:**
- `auth.md` — login/refresh/logout/me, cookies `vaemobility_*`.
- `catalog.md` — `GET /api/catalog*` público e `/api/admin/categories|products|media`.
- `src` de imagem pode ser URL `https://` (R2); front precisa relaxar Zod.

---

## [2026-09-18] — Bootstrap

**Tipo:** Criação

**Motivo:**
Estrutura inicial do contrato, sem módulos de negócio.

**Depois:**
- Convenções globais no README (`ApiResponse<T>`, JSON camelCase, enums como string).
- Códigos de erro em `errors.md`.
- Health público em `GET /health`.
