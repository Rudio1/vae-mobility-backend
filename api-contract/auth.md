# Auth

[← Voltar ao índice](./README.md)

Admin único. JWT em cookie HttpOnly; Swagger também aceita `Authorization: Bearer`.

**Autenticação:** login é público; `GET /me` exige JWT.

## Cookies

| Cookie | Conteúdo |
|--------|----------|
| `vaemobility_access_token` | JWT de acesso |
| `vaemobility_refresh_token` | Refresh token |

`withCredentials: true`. Não guardar tokens em `localStorage`.

---

### POST /api/auth/login

**Finalidade:** autenticar o admin.

**Autenticação:** Público

**Request body:**

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| email | string | sim | |
| password | string | sim | |

**Exemplo de request:**

```json
{ "email": "admin@vaemobility.com", "password": "..." }
```

**Resposta 200 — data:**

| Campo | Tipo |
|-------|------|
| accessToken | string |
| refreshToken | string |
| expiresAt | datetime ISO |
| user.id | Guid string |
| user.email | string |
| user.name | string |

**Respostas de erro:** `VALIDATION_ERROR` (400), `BUSINESS_ERROR` (400) credenciais inválidas.

---

### POST /api/auth/refresh

**Finalidade:** renovar o par de tokens.

**Autenticação:** refresh cookie ou body `{ "refreshToken": "..." }`

**Resposta 200:** mesmo shape do login.

---

### POST /api/auth/logout

**Finalidade:** revogar refresh e limpar cookies.

**Autenticação:** JWT ou refresh cookie

**Resposta:** 204

---

### GET /api/auth/me

**Finalidade:** sessão atual.

**Autenticação:** JWT

**Resposta 200 — data:** `{ "id", "email", "name" }`
