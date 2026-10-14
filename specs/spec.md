# Gerador de Senhas Fortes

## Problema
- As senhas das contas corporativas dos colaboradores de minha empresa são geradas com base na confiança de cada um e na maioria das vezes são senhas fracas e padronizadas, o que a longo prazo pode, de alguma maneira, comprometer os espaços corporativos da empresa.

## Objetivo
- Criação de senhas fortes e sem padrões para serem utilizadas em contas corporativas, como um ambiente de nuvem Azure ou uma conta do GitHub, que de jeito nenhum podem ser comprometidos.

## Usuários
- São colaboradores de uma empresa de construção de software.

## Histórias
1. O usuário deve conseguir criar uma senha;
2. O usuário deve receber um GUID ao criar uma senha;
3. O usuário deve conseguir buscar qualquer senha criada a partir do GUID retornado na criação da senha;
4. Se o usuário não conseguir criar a senha, deve ser exibido o erro de maneira clara porque ele não conseguiu criar a senha;
5. Se o usuário não conseguir encontrar a senha a partir do GUID, deve ser retornado 404;

## Regras de negócio
- RN01: Todas as senhas criadas devem retornar um GUID de identificação;
- RN02: Não deve ser gerado nenhuma senha igual;
- RN03: As senhas tem que ter caracteres especiais, 16 caracteres e não podem incluir espaços em branco.

## Casos de borda
- Não deve ser possível criar uma senha com menos de 16 caracteres;
- Não deve ser possível criar uma conta com mais de 16 caracteres;
- Não deve ter espaços em branco em uma senha;
- Ao criar uma senha, não existe a possibilidade de não ser retornado um GUID de identificação;

## Fora de escopo
- Criação de contas para usuários diferentes;
- As senhas do usuário não precisam estar em HASH;

## Critérios de aceite
- O usuário conseguir criar uma senha;
- O usuário conseguir buscar uma senha a partir do GUID;
- Os arquivos de build não podem subir no GitHub.
