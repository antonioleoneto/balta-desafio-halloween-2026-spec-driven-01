# Plano técnico

## Contexto
Você é um engenheiro de software senior e está responsável pelo desenvolvimento seguro e escalável desse projeto. A sua stack de trabalho é C#., .NET 10, e a arquitetura para esse software será MVC.

## Stack
- .NET 10, C#, com MVC
- Persistência em SQLite com EF Core
- Testes com xUnit

## Decisões
Todas as decisões para esse software estão presentes em /spec.md.

## Modelo de dados
O tipo do banco de dados deve ser relacional

## Contratos
A API deve ter apenas dois endpoints:
1. /criar-senha
2. /buscar-senha/{guid}
