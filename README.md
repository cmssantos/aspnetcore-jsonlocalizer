# 📦 Cms.AspNetCore.JsonLocalizer

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/356e8b5130994637b16075fcec31fbb8)](https://app.codacy.com/gh/cmssantos/aspnetcore-jsonlocalizer?utm_source=github.com&utm_medium=referral&utm_content=cmssantos/aspnetcore-jsonlocalizer&utm_campaign=Badge_Grade)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/SEU_PROJECT_ID)](https://www.codacy.com/gh/SEU_USUARIO/SEU_REPOSITORIO/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=SEU_USUARIO/SEU_REPOSITORIO&amp;utm_campaign=Badge_Grade)
[![Codacy Coverage](https://app.codacy.com/project/badge/Coverage/SEU_PROJECT_ID)](https://www.codacy.com/gh/SEU_USUARIO/SEU_REPOSITORIO/dashboard?utm_source=github.com&utm_medium=referral&utm_content=SEU_USUARIO/SEU_REPOSITORIO&utm_campaign=Badge_Coverage)
[![NuGet](https://img.shields.io/nuget/v/Cms.AspNetCore.JsonLocalizer.svg)](https://www.nuget.org/packages/Cms.AspNetCore.JsonLocalizer)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**Cms.AspNetCore.JsonLocalizer** é uma biblioteca leve e extensível para suporte à localização no ASP.NET Core usando arquivos JSON estruturados. Ela permite utilizar **chaves simples e aninhadas**, com suporte a **parâmetros de formatação**, **fallback de cultura** e resolução automática de cultura a partir do cabeçalho `Accept-Language`.

---

## ✨ Principais Recursos

-   ✅ Suporte a arquivos de localização em JSON
-   ✅ Resolução automática da cultura via HTTP
-   ✅ Chaves aninhadas via notação de ponto (`"errors.notFound.user"`)
-   ✅ Suporte a argumentos formatáveis (`"User {0} not found"`)
-   ✅ Fallback automático para cultura padrão
-   ✅ Cache em memória para performance
-   ✅ Injeção via `IJsonStringLocalizer`

---

## 📦 Instalação

```bash
dotnet add package Cms.AspNetCore.JsonLocalizer
```

---

## 🚀 Como Usar

### 1. Estrutura de Diretórios

Coloque seus arquivos JSON em uma pasta (por padrão `Resources/`):

```bash
Resources/
├── en-US.json
└── pt-BR.json
```

### 2. Exemplo de Arquivo JSON

#### `Resources/en-US.json`:

```json
{
    "errors": {
        "notFound": {
            "user": "User {0} not found."
        }
    },
    "greeting": "Hello, {0}!"
}
```

#### `Resources/pt-BR.json`:

```json
{
    "errors": {
        "notFound": {
            "user": "Usuário {0} não encontrado."
        }
    },
    "greeting": "Olá, {0}!"
}
```

---

### 3. Registro no `Program.cs`

```csharp
builder.Services.AddJsonLocalization(options =>
{
    options.ResourcesPath = "Resources";
    options.DefaultCulture = "en-US";
});
```

---

### 4. Injeção e Uso

#### Em uma Controller ou Serviço:

```csharp
public class UserController : ControllerBase
{
    private readonly IJsonStringLocalizer _localizer;

    public UserController(IJsonStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    [HttpGet("notfound/{name}")]
    public IActionResult NotFoundMessage(string name)
    {
        var message = _localizer["errors.notFound.user", name];
        return NotFound(message);
    }
}
```

---

## 🧪 Testes Automatizados

Os testes unitários estão estruturados usando arquivos de exemplo que simulam múltiplas culturas e categorias (namespaces). Veja a pasta `TestResources/` com arquivos como:

```
TestResources/
├── Users.en-US.json
├── Users.pt-BR.json
├── Errors.en-US.json
└── Errors.pt-BR.json
```

Exemplo de teste:

```csharp
[Theory]
[InlineData("Users", "pt-BR", "greeting", "Olá, {0}!")]
[InlineData("Users", "en-US", "greeting", "Hello, {0}!")]
public void GetValue_FromNamespaceFile(string ns, string culture, string key, string expected)
{
    var accessor = new JsonLocalizationFileAccessor(resourcesPath);
    var localizer = new JsonStringLocalizer(accessor, culture, ns);

    var result = localizer[key];

    Assert.Equal(expected, result);
}
```

---

## 🛠 Arquitetura

### 🔑 Interface

```csharp
public interface IJsonStringLocalizer
{
    string this[string key] { get; }
    string this[string key, params object[] arguments] { get; }
}
```

### ⚙️ Componentes

| Classe                         | Responsabilidade                                                     |
| ------------------------------ | -------------------------------------------------------------------- |
| `JsonStringLocalizer`          | Localiza strings a partir de arquivos JSON, com suporte a formatação |
| `JsonLocalizationFileAccessor` | Carrega arquivos JSON e faz cache na memória                         |
| `JsonLocalizationOptions`      | Configurações da localização (cultura padrão, pasta, etc.)           |
| `ServiceCollectionExtensions`  | Extensão para `IServiceCollection` registrar a dependência           |

---

## 🌐 Suporte a Cultura

-   A cultura é extraída automaticamente do cabeçalho `Accept-Language`, com fallback para a cultura padrão (`DefaultCulture`).
-   Exemplo de header HTTP suportado:
    ```
    Accept-Language: pt-BR
    ```

---

## 🧩 Suporte a Namespaces (opcional)

Você pode organizar seus arquivos por namespace/categoria para granularidade:

```bash
Resources/
├── Users.en-US.json
├── Users.pt-BR.json
├── Errors.en-US.json
└── Errors.pt-BR.json
```

Crie um `JsonStringLocalizer` passando o namespace:

```csharp
var localizer = new JsonStringLocalizer(accessor, "pt-BR", "Users");
var message = localizer["greeting"];
```

---

## 🧼 Boas Práticas

-   Use nomes de arquivos e culturas consistentes (`xx-YY.json`).
-   Prefira chaves semânticas e organizadas, como `"errors.notFound.user"`.
-   Evite valores hardcoded em seu código. Use sempre o localizador injetado.

---

## 📄 Licença

Este projeto é licenciado sob a [MIT License](LICENSE).
