# TaskFlow - Projeto DDD em C#

## Descrição

O TaskFlow é uma aplicação console desenvolvida em C# para demonstrar a aplicação de conceitos de Orientação a Objetos, Domain-Driven Design, SOLID, GRASP e testes unitários.

O sistema permite gerenciar projetos e tarefas, incluindo:

- criação de projetos;
- inclusão de membros no projeto;
- criação de tarefas;
- atribuição de responsáveis;
- alteração de status da tarefa;
- conclusão de tarefas;
- verificação de tarefas atrasadas;
- bloqueio de criação de tarefas em projetos arquivados.

## Estrutura da solução

TaskFlow
|
|-- src
|   |-- TaskFlow.Domain
|   |-- TaskFlow.Application
|   |-- TaskFlow.Infrastructure
|   |-- TaskFlow.ConsoleApp
|
|-- tests
|   |-- TaskFlow.Tests
|
|-- TaskFlow.sln
|-- README.md

## Projetos da solução

### TaskFlow.Domain

Contém o núcleo do domínio da aplicação.

Este projeto concentra as principais regras de negócio e os conceitos de Domain-Driven Design.

Inclui:

- Entities;
- Value Objects;
- Aggregates;
- Repositories;
- Factories;
- Domain Services;
- Anti-Corruption Layer;
- regras de negócio.

Principais pastas:

- Common
- Projects
- Tasks
- Services
- Rules
- Acl
- Exceptions

### TaskFlow.Application

Contém os Application Services, responsáveis por coordenar os casos de uso da aplicação.

O Application Service não deve concentrar regras de negócio complexas. Ele apenas organiza o fluxo da operação, chamando factories, repositories e domain services.

Principais classes:

- ProjetoApplicationService
- TarefaApplicationService

### TaskFlow.Infrastructure

Contém detalhes técnicos de infraestrutura.

Neste projeto foram implementados repositories em memória e o adapter da Anti-Corruption Layer.

Principais classes:

- InMemoryProjetoRepository
- InMemoryTarefaRepository
- ProjetoAclAdapter

### TaskFlow.ConsoleApp

Aplicação console utilizada para demonstrar o funcionamento do domínio.

O Program.cs cria um projeto, adiciona membros, cria uma tarefa, atribui responsável, inicia a tarefa, conclui a tarefa e demonstra uma regra de negócio impedindo a criação de tarefa em projeto arquivado.

### TaskFlow.Tests

Projeto de testes unitários criado com xUnit.

Este projeto valida as principais regras de negócio do domínio e dos serviços da aplicação.

Inclui testes para:

- Entities;
- Value Objects;
- Factories;
- Domain Services;
- Application Services;
- Repositories em memória;
- Anti-Corruption Layer;
- exceções de domínio;
- cenários de sucesso e erro.

Principais pastas:

- Acl
- Application
- Common
- Factories
- Infrastructure
- Projects
- Services
- Tasks

## Como executar a aplicação

Na raiz do projeto, execute:

    dotnet build

Depois execute:

    dotnet run --project src/TaskFlow.ConsoleApp/TaskFlow.ConsoleApp.csproj

## Como executar os testes

Para executar todos os testes da solução:

    dotnet test

Para executar apenas o projeto de testes:

    dotnet test tests/TaskFlow.Tests/TaskFlow.Tests.csproj

## Como executar os testes com cobertura

O projeto utiliza Coverlet para gerar cobertura de testes.

Execute:

    dotnet test tests/TaskFlow.Tests/TaskFlow.Tests.csproj \
    /p:CollectCoverage=true \
    /p:CoverletOutput=tests/TaskFlow.Tests/TestResults/coverage.opencover.xml \
    /p:CoverletOutputFormat=opencover

O arquivo de cobertura será gerado em:

    tests/TaskFlow.Tests/TestResults/coverage.opencover.xml

## Como gerar relatório HTML da cobertura

Caso o ReportGenerator esteja instalado, execute:

    reportgenerator \
    -reports:tests/TaskFlow.Tests/TestResults/coverage.opencover.xml \
    -targetdir:tests/TaskFlow.Tests/TestResults/CoverageReport \
    -reporttypes:Html

Depois abra o arquivo:

    tests/TaskFlow.Tests/TestResults/CoverageReport/index.html

No macOS, é possível abrir com:

    open tests/TaskFlow.Tests/TestResults/CoverageReport/index.html

## Regras de negócio implementadas

A aplicação implementa as seguintes regras de negócio:

1. Um projeto deve possuir nome válido.
2. Um projeto deve possuir pelo menos um membro responsável.
3. Um projeto arquivado não pode ser alterado.
4. Um projeto arquivado não pode receber novas tarefas.
5. Uma tarefa sempre pertence a um projeto.
6. Uma tarefa nova inicia com status Backlog.
7. Uma tarefa só pode ser atribuída a um usuário que pertence ao projeto.
8. Uma tarefa só pode ser atribuída se o projeto estiver ativo.
9. Uma tarefa não pode ser concluída sem responsável.
10. Uma tarefa só pode ser concluída se estiver em andamento.
11. Uma tarefa concluída não pode ser alterada.
12. Uma tarefa é considerada atrasada quando o prazo venceu e ela ainda não foi concluída.

## Regras testadas

O projeto de testes cobre regras como:

| Classe ou componente | Regras testadas |
|---|---|
| Projeto | criação de projeto ativo, adição de membros, remoção de membros e arquivamento |
| Tarefa | criação em Backlog, atribuição de responsável, início, conclusão e bloqueio de alterações |
| VerificadorPrazoService | identificação de tarefas atrasadas |
| AlocacaoTarefaService | atribuição somente para membros de projetos ativos |
| ConclusaoTarefaService | validação das regras antes de concluir uma tarefa |
| ProjetoAclAdapter | integração entre contexto de tarefas e contexto de projetos |
| Repositories em memória | adição, busca, atualização e validações |
| Value Objects | validação de valores inválidos e comparação por valor |
| Factories | criação de objetos em estado válido |

## Conceitos de Orientação a Objetos demonstrados

| Conceito | Aplicação |
|---|---|
| Encapsulamento | Propriedades com private set e alteração de estado por métodos de domínio |
| Abstração | Interfaces como IProjetoRepository, ITarefaRepository, IProjetoAcl, IProjetoFactory, ITarefaFactory e IRegraConclusaoTarefa |
| Herança | Classes base Entity e ValueObject |
| Polimorfismo | Interface IRegraConclusaoTarefa com múltiplas implementações |

## Conceitos de DDD demonstrados

| Conceito | Aplicação |
|---|---|
| Ubiquitous Language | Uso de termos como Projeto, Tarefa, MembroProjeto, Prazo, Prioridade e Responsável |
| Entity | Projeto, MembroProjeto e Tarefa |
| Value Object | NomeProjeto, TituloTarefa, DescricaoTarefa e Prazo |
| Aggregate Root | Projeto e Tarefa |
| Repository | IProjetoRepository e ITarefaRepository |
| Factory | ProjetoFactory e TarefaFactory |
| Domain Service | AlocacaoTarefaService, VerificadorPrazoService e ConclusaoTarefaService |
| Anti-Corruption Layer | IProjetoAcl e ProjetoAclAdapter |
| Context Map | Integração entre Gestão de Tarefas e Gestão de Projetos por ACL |

## Conceitos de SOLID demonstrados

| Princípio | Aplicação |
|---|---|
| SRP | Classes com responsabilidades específicas, como Projeto, Tarefa, Factories, Services e Repositories |
| OCP | Novas regras de conclusão podem ser adicionadas implementando IRegraConclusaoTarefa |
| LSP | Implementações podem substituir abstrações como ITarefaRepository e IProjetoRepository |
| ISP | Interfaces pequenas e específicas, como IProjetoAcl, IProjetoFactory e ITarefaFactory |
| DIP | Application Services dependem de abstrações, e não de classes concretas |

## Conceitos de GRASP demonstrados

| Padrão | Aplicação |
|---|---|
| Information Expert | Tarefa controla seu próprio status e conclusão |
| Creator | ProjetoFactory e TarefaFactory criam objetos válidos |
| Controller | ProjetoApplicationService e TarefaApplicationService coordenam casos de uso |
| Low Coupling | Uso de interfaces reduz dependências entre classes |
| High Cohesion | Cada classe possui responsabilidade clara |
| Polymorphism | Regras de conclusão implementam IRegraConclusaoTarefa |
| Pure Fabrication | Repositories separam persistência do domínio |
| Indirection | IProjetoAcl atua como intermediária entre contextos |
| Protected Variations | Interfaces protegem contra mudanças futuras |

## Fluxo demonstrado no ConsoleApp

O ConsoleApp executa o seguinte fluxo:

1. Cria os repositories em memória.
2. Cria um projeto usando ProjetoApplicationService.
3. Adiciona um membro ao projeto.
4. Cria a ACL de projetos.
5. Cria os domain services.
6. Cria uma tarefa usando TarefaApplicationService.
7. Atribui a tarefa a um membro do projeto.
8. Inicia a tarefa.
9. Verifica se a tarefa está atrasada.
10. Conclui a tarefa.
11. Arquiva o projeto.
12. Tenta criar uma nova tarefa no projeto arquivado.
13. Exibe a regra de negócio impedindo a operação.

## Como validar o funcionamento

Após executar a aplicação, o console deverá exibir informações parecidas com:

TaskFlow - DDD Console Application

Project created:
Name: Projeto TaskFlow
Status: Ativo

Task created:
Title: Implementar domínio
Priority: Alta
Status: Backlog

Task after assignment and start:
Status: EmAndamento
Is overdue: False

Task completed:
Status: Concluida

Trying to create a task in an archived project...
Business rule applied: Cannot create a task for an archived or non-existing project.

Application finished successfully.

## Observação

A persistência foi implementada em memória para manter o foco nos conceitos de Orientação a Objetos, Domain-Driven Design, SOLID, GRASP e testes unitários.

Em uma evolução futura, os repositories em memória poderiam ser substituídos por repositories com banco de dados, sem alterar as regras principais do domínio.
