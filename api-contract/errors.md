# Padrão de erros

Referência completa de como a API sinaliza falhas. Use junto com o [envelope de resposta](./README.md#envelope-de-resposta) descrito no README.

[← Voltar ao índice](./README.md)

---

## Regra geral

Quando a API retorna erro **com corpo JSON**, o formato é sempre:

```json
{
  "success": false,
  "data": null,
  "error": { }
}
```

| Campo raiz | Em erro | Descrição |
|------------|---------|-----------|
| `success` | `false` | Indica falha |
| `data` | `null` | Sem payload |
| `error` | objeto | Detalhes do erro |

---

## Estrutura do objeto `error`

| Campo | Tipo | Sempre presente | Descrição |
|-------|------|-----------------|-----------|
| `code` | string | Sim | Código categorizado (ver tabela abaixo) |
| `message` | string | Sim | Mensagem legível para exibir ao usuário |
| `fields` | array \| null | Não | Lista de erros por campo; **somente** em `VALIDATION_ERROR` |

### Estrutura de `fields[]`

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `field` | string | Nome da propriedade com erro (ex.: `Email`) |
| `message` | string | Mensagem específica do campo |

> **Atenção:** nomes em `field` seguem o nome da propriedade C# (PascalCase), não camelCase do JSON de request.

---

## Códigos de erro e HTTP

| `error.code` | HTTP | Quando ocorre |
|--------------|------|---------------|
| `VALIDATION_ERROR` | **400** | Request inválido (FluentValidation ou validação de domínio) |
| `BUSINESS_ERROR` | **400** ou **422** | Regra de negócio violada (ver nota abaixo) |
| `NOT_FOUND` | **404** | Recurso não encontrado |
| `UNAUTHORIZED` | **403** | Acesso negado por regra de autorização de negócio |
| `INTERNAL_ERROR` | **500** | Erro inesperado no servidor |

### Nota sobre `BUSINESS_ERROR`

Depende de **como** o erro foi gerado:

| Origem | HTTP | Exemplo |
|--------|------|---------|
| Handler retorna `Result.Fail(...)` | **400** | Credenciais inválidas, documento duplicado |
| Exceção `BusinessException` / `DomainException` (middleware) | **422** | Violação de invariante de domínio |

No front-end, trate **400 e 422** da mesma forma quando `error.code === "BUSINESS_ERROR"`: exibir `error.message`.

### Autenticação JWT (caso especial)

Quando o endpoint exige `[Authorize]` e o token está **ausente, expirado ou inválido**, o ASP.NET retorna:

- **HTTP 401** — geralmente **sem** o envelope `ApiResponse` (resposta padrão do middleware JWT)

O front deve tratar **401** como sessão inválida: tentar refresh ou redirecionar para login.

> `UNAUTHORIZED` (403) é diferente: a API autenticou o usuário, mas a **regra de negócio** negou a operação.
