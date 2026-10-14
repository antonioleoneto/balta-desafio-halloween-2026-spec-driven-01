# Tarefas

| ID | Tarefa | Origem | Depende de | Concluída quando |
|---|---|---|---|---|
| T-01 | Criar projeto e estrutura de pastas | Constituição | | Projeto compila |
| T-02 | Criar dominios, serviços e controllers | Constituição, Especificação e Planejamento  | T-01 | Projeto compila sem warnings e erros |
| T-03 | Criar testes unitários para cada serviço e controller | Constituição, Especificação e Planejamento | T-02 | Cobertura de testes em 100% |
| T-04 | Criar testes unitários para cada regra de negócio | Especificação | | Temos testes prontos para desenvolver as regras de negócio |
| T-05 | Desenvolver as regras de negocio RN01, RN02, RN03 e RN04 | Constituição, Especificação e Planejamento | T-04 | Cobertura de testes em 100% |
| T-06 | Criar testes unitários para os casos de borda | Especificação | | Cobertura de testes em 100% |
| T-07 | Aplicar as correções da nova constitution. Foi acrescentado uma nova regra para a arquitetura tanto de Modelo quanto de Serviço. Além disso, adicionei uma nova regra na convenção de commits | Especificação | | Sem arquivos pendentes de commit, projeto compila, arquivos separados conforme constitution |
| T-08 | Aplicar as correções da nova spec.md. Foi modificado a RN03, pois estava muito vago o "padrão de letras, numeros ou simbolos iguais" e incrementei um exemplo. | Especificação | T-04 | Cobrir os testes |
| T-09 | Aplicar as correções da nova spec.md. Foi modificado a RN03, pois o exemplo inserido tinha sequencia de caracteres. | Especificação | T-04 | Cobrir os testes |
| T-10 | Aplicar as correções da nova spec.md. Foi modificado a RN03, pois para um projeto de treino, envolvia complexidade demais. A antiga RN04 agora é a RN03. Remover qualquer resquicio da antiga RN03 no projeto. | Especificação | T-04 | Cobrir os testes |
| T-11 | Eu adicionei uma ferramenta de cobertura de testes em plan.md, implemente essa ferramenta. | Planejamento | T-06 | Cobertura de testes em 100%|
| T-12 | Construir um serviço para validar as senhas geradas, se elas seguem os padrões das regras de negócio e casos de borda. | Especificação | T-05 | Cobertura de testes em 100% e código compila sem warnings e erros|
| T-13 | Em Planejamento, em decisões, adicionei uma linha para implementar o swagger para conseguir testar os endpoints no navegador | Planejamento | | Swagger funcionando no navegador |
| T-14 | Em Planejamento, em decisões, adicionei uma linha para não subir os arquivos de build no github, adicione no .gitignore | Planejamento | | Sem arquivos de build como pendentes em arquivos mudados |
| T-15 | Em Planejamento, eu acrescentei um novo tópico que é "Documentação", siga os passo a passos deste tópico para construir o readme.md do projeto. | Planejamento | | Arquivo Readme.md de /src pronto |
| T-16 | Remover commit "docs: cria README do projeto conforme seção Documentação do plano (T-15)" gerado a partir do Claude. | Constitution | | Sem collab de commits em meu git hub |

