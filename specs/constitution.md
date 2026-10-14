# Constituição do projeto

## Arquitetura
- Para a arquitetura desse projeto, utilizaremos MVC, porem, sem a necessidade de ter uma camada Visão, pois não teremos frontend:
    - A ideia é ter muito bem controlado as regras de negócio e funcionalidades relacionadas ao objetivo principal do software em Modelo. Os modelos devem ser separados entre pastas diferentes para tornar mais facil a visualização entre response, interface, modelo e exceção.
    - Em Services, deve ficar todas as regras de negócio para que não seja exibido nada disso em controller. Deve ser separado entre pastas diferentes para tornar mais fácil a visualização de interfaces e services.
    - Já em Controller, vamos lidar com a interação entre o modelo e o que será retornado ao usuário, sempre se atentando a não exibir dados sensíveis sem necessidade.
- Endpoint não acessa banco diretamente, sempre via serviço de aplicação
- Regra de negócio não vive no endpoint nem no handler HTTP
- Nenhuma dependência nova entra sem justificativa escrita no plano
- O Swagger deve ser implementado para testar os endpoints da API de forma mais simples e sem barreiras.


## Qualidade
- O código deve compilar sem erros e warnings. 
- Para cada regra de negócio, deve ser criado um teste unitário.
- Para cada endpoint, deve ser criado um teste unitário que verifique se o caminho de sucesso retorna o status proposto e um teste unitário que verifique se o caminho de erro retorna o status proposto.
- É esperado que o software seja:
    - Repetitível: Resultados idênticos devem ser obtidos ao avaliar esse software várias vezes, utilizando a mesma especificação da avaliação e o mesmo avaliador;
    - Reprodutível: Resultados idênticos devem ser obtidos ao avaliar um mesmo produto repetidas vezes, considerando a mesma especificação da avaliação e sendo realizada por outro avaliador;
    - Objetivo: Os resultados da avaliação devem ser baseados em fatos e evidências, isto é, não devem ser influenciados por sentimentos ou opiniões do avaliador.
- Erro de validação sempre retorna 400 com ProblemDetails

## Convenções
- Convenções de código:
    - Todos métodos, variáveis e constantes devem ser tipados.
    - Nomes de classes, métodos, enumerações, campos públicos, propriedades públicas, namespaces: PascalCase.
    - Nomes de variáveis locais, parâmetros: camelCase.
    - Nomes de campos e propriedades privadas, protegidas, internas e protegidas: _camelCase.
    - Para constantes: SCREAMING_SNAKE_CASE.
    - A convenção de nomenclatura não é afetada por modificadores como const, static, readonly, etc.
    - Para a caixa, uma “palavra” é qualquer coisa escrita sem espaços internos, incluindo siglas. Por exemplo, MyRpc em vez de MyRPC.
    - Os nomes das interfaces começam com I, por exemplo. Interface.
- Convenções de arquivos:
    - Nomes de arquivos e nomes de diretórios são PascalCase, por exemplo. Meu Arquivo.cs.
    - Sempre que possível, o nome do arquivo deve ser igual ao nome da classe principal no arquivo, por exemplo. Minha Classe.cs.
    - Em geral, prefira uma classe principal por arquivo.
- Convenções de organização:
    - Modificadores ocorrem na seguinte ordem: público protegido interno privado novo abstrato substituição virtual selado estático somente leitura externo inseguro volátil assíncro.
    - O namespace usando declarações vai no topo, antes de qualquer namespace. usando a ordem de importação é alfabética, além das importações do sistema que sempre vão primeiro.
    - Pedido do membro da classe:
        - Membros da classe do grupo na seguinte ordem:
            - Classes aninhadas, enums, delegados e eventos.
            - Campos estáticos, const e somente leitura.
            - Campos e propriedades.
            - Construtores e finalizadores.
            - Métodos.
        - Dentro de cada grupo, os elementos devem estar na seguinte ordem:
            - Público.
            - Interno.
            - Interno protegido.
            - Protegido.
            - Privado.
        - Sempre que possível, agrupe as implementações da interface.
- Convenções de comentários:
    - Comentários de blocos para fornecer descrições de arquivos, métodos, estruturas de dados e algoritmos.
- Convenção de commits:
    - Cada tarefa deve ser um commit e quando a tarefa for concluida, já deve vir a mensagem de commit com a seguinte convenção:
        - fix: para solução de problemas;
        - feat: para novas features;
        - docs: para mudanças em documentações;
        - refactor: para refatorações em códigos que não alteraram funcionalidades, apenas trouxeram melhoras em performances.
        - build: para quando são realizadas modificações em arquivos de build e dependências.
        - test: para quando são construidos ou alterados arquivos de testes unitários.
        - chore: para atualizações de tarefas de build, configurações de administrador, pacotes... como por exemplo adicionar um pacote no gitignore.