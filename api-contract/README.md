# VaeMobility API — Contrato para Front-end

> **Manutenção obrigatória:** atualize este diretório sempre que criar, alterar ou remover uma rota, DTO, validador ou comportamento de autenticação. Registre a mudança em [CHANGELOG.md](./CHANGELOG.md).

Pasta autocontida para integração no projeto front-end (cópia, submodule, symlink ou monorepo).

## Base URL

| Ambiente | URL |
|----------|-----|
| Desenvolvimento | `http://localhost:5080` |
| Produção | `{BASE_URL}` (definir no front-end) |

Swagger interativo (referência técnica ao vivo): `{BASE_URL}/swagger`

## Índice

| Arquivo | Conteúdo |
|---------|----------|
| [auth.md](./auth.md) | Login, refresh, logout, sessão |
| [catalog.md](./catalog.md) | Catálogo público e CRUD admin |
| [errors.md](./errors.md) | Padrão de erros, códigos HTTP e exemplos |
| [CHANGELOG.md](./CHANGELOG.md) | Histórico de alterações do contrato |

Novos módulos devem ganhar um `.md` próprio e entrar neste índice.

## Convenções globais

### Envelope de resposta

Todas as respostas seguem o formato `ApiResponse<T>`:

```json
{
  "success": true,
  "data": { },
  "error": null
}
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `success` | boolean | `true` quando a operação foi bem-sucedida |
| `data` | T \| null | Payload da resposta; `null` em erro |
| `error` | object \| null | Detalhes do erro; `null` em sucesso |

### Padrão de erros

Toda falha com corpo JSON segue `success: false`, `data: null` e o objeto `error`:

```json
{
  "success": false,
  "data": null,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Validation failed.",
    "fields": [
      { "field": "Email", "message": "'Email' is not a valid email address." }
    ]
  }
}
```

| Campo de `error` | Tipo | Descrição |
|------------------|------|-----------|
| `code` | string | Código categorizado (`VALIDATION_ERROR`, `BUSINESS_ERROR`, etc.) |
| `message` | string | Mensagem para exibir ao usuário |
| `fields` | array \| null | Erros por campo; presente apenas em `VALIDATION_ERROR` |

**Resumo dos códigos** (detalhes e exemplos em [errors.md](./errors.md)):

| `error.code` | HTTP | Descrição |
|--------------|------|-----------|
| `VALIDATION_ERROR` | 400 | Request inválido; usar `error.fields` no formulário |
| `BUSINESS_ERROR` | 400 ou 422 | Regra de negócio; exibir `error.message` |
| `NOT_FOUND` | 404 | Recurso não encontrado |
| `UNAUTHORIZED` | 403 | Acesso negado (usuário autenticado, sem permissão) |
| `INTERNAL_ERROR` | 500 | Erro interno |

**Caso especial — JWT inválido/ausente:** endpoints `[Authorize]` retornam **401** sem envelope `ApiResponse` (padrão ASP.NET). Tratar como sessão expirada (refresh ou logout).

### Autenticação

Endpoints protegidos leem o JWT do cookie HttpOnly `vaemobility_access_token` (ou header `Authorization: Bearer` no Swagger).

| Cookie | Conteúdo |
|--------|----------|
| `vaemobility_access_token` | JWT de acesso |
| `vaemobility_refresh_token` | Refresh token |

Definidos em login/refresh; removidos em logout. Frontend: `withCredentials: true`.

### Serialização JSON

- Propriedades em **camelCase**
- Enums como **string**
- Datas em ISO 8601 (`"2026-09-18T20:00:00Z"`)

### CORS

Origens via `Cors:AllowedOrigins` (credenciais habilitadas).

### Health

`GET /health` — público, sem envelope `ApiResponse`.
