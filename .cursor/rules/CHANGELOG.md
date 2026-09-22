# Changelog — Cursor Rules (VaeMobility)

Registro de alterações nas regras do projeto em `.cursor/rules/`.

Sempre que uma rule for criada, alterada ou removida, **adicione uma entrada no topo** deste arquivo (mais recente primeiro).

---

## [2026-09-18] — CMS catálogo + schema SQL

**Tipo:** Alteração / Criação

**Motivo:**
Backend passou a ser o CMS do site (Auth, Catalog, Media). Schema manual, sem EF Migrations.

**Depois:**
- Contextos em `02-folder-structure`: `Generic`, `Auth`, `Catalog`, `Media`.
- `20-migrations` agora **proíbe** EF Migrations e aponta para `sql/`.
- Nova rule `25-product-context` (alwaysApply) aponta para `docs/` e `api-contract/`.

**Impacto:**
Não criar pasta Migrations. Qualquer tabela nova nasce em `sql/00N_*.sql`.

**Autor:** cms-catalogo

---

## [2026-09-18] — Bootstrap das rules

**Tipo:** Criação

**Motivo:**
Início do `vaemobility-backend` com o mesmo padrão de Clean Architecture do AgroPulse, sem copiar código de negócio.

**Depois:**
- 23 rules portadas para o prefixo `VaeMobility`.
- `01-architecture`, `02-folder-structure` e `24-business-rules-ownership` com `alwaysApply: true`.
- Contextos atuais: apenas `Generic`.
- `17-use-cases` usa Domain Service no Handler (não injeta repositório).

**Impacto:**
A IA deve seguir estas rules em qualquer feature futura.

**Autor:** bootstrap
