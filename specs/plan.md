# Plano técnico

## Contexto
Você é um engenheiro de software senior e está responsável pelo desenvolvimento seguro e escalável desse projeto. A sua stack de trabalho é C#., .NET 10, e a arquitetura para esse software será MVC.

## Stack
- .NET 10, C#, com MVC
- Persistência em SQLite com EF Core
- Testes com xUnit

## Cobertura de testes
- Coverlet

## Decisões
Todas as decisões para esse software estão presentes em /spec.md.

## Modelo de dados
O tipo do banco de dados deve ser relacional

## Contratos
A API deve ter apenas dois endpoints:
1. /criar-senha
2. /buscar-senha/{guid}

## Documentação
O arquivo README.md em /src do projeto deve obrigatoriamente seguir a seguinte estrutura para garantir a consistência e facilidade de onboarding:
1. Título e Descrição: Resumo claro do propósito da aplicação.
2. Pré-requisitos: Ferramentas necessárias para rodar a API.
3. Instalação e Setup: Passo a passo para rodar o projeto localmente.
4. Como Executar: Comandos para iniciar em modo desenvolvimento.
5. Documentação da API (Swagger): Instruções de como acessar o Swagger UI localmente (ex: http://localhost:8080/docs) e onde encontrar o arquivo de spec.
6. Scripts Disponíveis: Lista de comandos úteis (testes, linters, geração de código do spec).
