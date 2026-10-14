# Gerador de Senhas Fortes

API REST para gerar senhas fortes e sem padrões, destinadas a contas corporativas críticas (como ambientes de nuvem Azure ou contas do GitHub). Cada senha criada recebe um GUID de identificação, que depois pode ser usado para buscá-la.

Toda senha gerada segue as regras de negócio da especificação ([specs/spec.md](../specs/spec.md)):

- **RN01:** toda senha criada retorna um GUID de identificação;
- **RN02:** nenhuma senha é gerada em duplicidade;
- **RN03:** a senha tem exatamente 16 caracteres, contém caracteres especiais e não contém espaços em branco.

A aplicação segue a arquitetura MVC sem camada de Visão: as regras ficam em `Models` e `Services`, e os `Controllers` apenas traduzem o resultado em respostas HTTP. Os dados são persistidos em SQLite via EF Core.

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0) (`dotnet --version` deve retornar `10.x`)
- Git

Não é necessário instalar o SQLite: o banco é um arquivo local (`gerador-senhas.db`) criado automaticamente na primeira execução.

## Instalação e Setup

1. Clone o repositório:

   ```bash
   git clone <url-do-repositorio>
   cd balta-desafio-halloween-2026-spec-driven-01/src
   ```

2. Restaure as dependências:

   ```bash
   dotnet restore GeradorSenhas.slnx
   ```

3. Compile a solução (o build trata warnings como erros):

   ```bash
   dotnet build GeradorSenhas.slnx
   ```

A string de conexão fica em `GeradorSenhas.Api/appsettings.json` (`ConnectionStrings:GeradorSenhas`). O banco e suas tabelas são criados automaticamente ao iniciar a API; nenhuma migração manual é necessária.

## Como Executar

Inicie a API em modo desenvolvimento (a partir de `src/`):

```bash
dotnet run --project GeradorSenhas.Api --launch-profile http
```

A API ficará disponível em `http://localhost:5203` e o navegador abrirá direto no Swagger UI.

Para executar também com HTTPS:

```bash
dotnet run --project GeradorSenhas.Api --launch-profile https
```

Nesse caso, a API responde em `https://localhost:7052` e `http://localhost:5203`.

### Endpoints

| Método | Rota | Descrição | Respostas |
|---|---|---|---|
| `POST` | `/criar-senha` | Gera uma nova senha e retorna seu GUID | `201` com `{ "id": "<guid>" }`, `400` com ProblemDetails |
| `GET` | `/buscar-senha/{guid}` | Busca uma senha pelo GUID | `200` com `{ "id": "<guid>", "valor": "<senha>" }`, `404` |

Exemplo com `curl`:

```bash
curl -X POST http://localhost:5203/criar-senha
curl http://localhost:5203/buscar-senha/<guid-retornado>
```

## Documentação da API (Swagger)

O Swagger é habilitado **somente no ambiente de desenvolvimento** (`ASPNETCORE_ENVIRONMENT=Development`, já definido nos perfis de `launchSettings.json`).

Com a API em execução:

- **Swagger UI:** [http://localhost:5203/swagger](http://localhost:5203/swagger)
- **Arquivo de spec (OpenAPI JSON):** [http://localhost:5203/swagger/v1/swagger.json](http://localhost:5203/swagger/v1/swagger.json)

O documento OpenAPI é gerado em tempo de execução a partir dos controllers pelo Swashbuckle; não há arquivo de spec versionado no repositório. A configuração fica em `GeradorSenhas.Api/Configurations/SwaggerConfiguration.cs`.

Para salvar o spec localmente:

```bash
curl http://localhost:5203/swagger/v1/swagger.json -o swagger.json
```

## Scripts Disponíveis

Todos os comandos são executados a partir de `src/`.

| Comando | Descrição |
|---|---|
| `dotnet restore GeradorSenhas.slnx` | Restaura as dependências |
| `dotnet build GeradorSenhas.slnx` | Compila a solução (falha se houver warnings) |
| `dotnet run --project GeradorSenhas.Api` | Inicia a API em modo desenvolvimento |
| `dotnet watch --project GeradorSenhas.Api` | Inicia a API recompilando a cada alteração |
| `dotnet test GeradorSenhas.slnx` | Executa os testes xUnit com cobertura do Coverlet; falha se linhas, branches ou métodos ficarem abaixo de 100% |
| `dotnet format GeradorSenhas.slnx --verify-no-changes` | Verifica a formatação e o estilo do código (lint) |
| `dotnet format GeradorSenhas.slnx` | Corrige a formatação e o estilo do código |

O relatório de cobertura é gerado em formato Cobertura em `GeradorSenhas.Tests/coverage.cobertura.xml`.

**Geração de código a partir do spec:** o projeto não possui geração de código a partir do spec; o fluxo é o inverso (o spec OpenAPI é gerado a partir do código). Para gerar um cliente a partir dele, use o arquivo `swagger.json` exportado na seção anterior com a ferramenta de sua preferência.
