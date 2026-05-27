# TaskFlow - Projeto DDD em C#

## Descrição

O TaskFlow é uma aplicação console desenvolvida em C# para demonstrar a aplicação de conceitos de Orientação a Objetos e Domain-Driven Design.

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

```text
TaskFlow
|
|-- src
|   |-- TaskFlow.Domain
|   |-- TaskFlow.Application
|   |-- TaskFlow.Infrastructure
|   |-- TaskFlow.ConsoleApp
|
|-- TaskFlow.sln
|-- README.md
|-- ENTREGA_PARTE_2.md
```

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

Principal classe:

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

## Como executar

Na raiz do projeto, execute:

    dotnet build

Depois execute:

    dotnet run --project src/TaskFlow.ConsoleApp/TaskFlow.ConsoleApp.csproj

## Conceitos de Orientação a Objetos demonstrados

| Conceito | Aplicação |
|---|---|
| Encapsulamento | Propriedades com private set e alteração de estado por métodos de domínio |
| Abstração | Interfaces como IProjetoRepository, ITarefaRepository, IProjetoAcl e IRegraConclusaoTarefa |
| Herança | Classes base Entity e ValueObject |
| Polimorfismo | Interface IRegraConclusaoTarefa com múltiplas implementações |

## Conceitos de DDD demonstrados

| Conceito | Aplicação |
|---|---|
| Ubiquitous Language | Uso de termos como Projeto, Tarefa, MembroProjeto, Prazo, Prioridade e Responsavel |
| Entity | Projeto, MembroProjeto e Tarefa |
| Value Object | NomeProjeto, TituloTarefa, DescricaoTarefa e Prazo |
| Aggregate Root | Projeto e Tarefa |
| Repository | IProjetoRepository e ITarefaRepository |
| Factory | ProjetoFactory e TarefaFactory |
| Domain Service | AlocacaoTarefaService, VerificadorPrazoService e ConclusaoTarefaService |
| Anti-Corruption Layer | IProjetoAcl e ProjetoAclAdapter |
| Context Map | Integração entre Gestão de Tarefas e Gestão de Projetos por ACL |

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

## Fluxo demonstrado no ConsoleApp

O ConsoleApp executa o seguinte fluxo:

1. Cria os repositories em memória.
2. Cria um projeto usando ProjetoFactory.
3. Adiciona um membro ao projeto.
4. Salva o projeto no repository.
5. Cria a ACL de projetos.
6. Cria os domain services.
7. Cria uma tarefa usando TarefaApplicationService.
8. Atribui a tarefa a um membro do projeto.
9. Inicia a tarefa.
10. Verifica se a tarefa está atrasada.
11. Conclui a tarefa.
12. Arquiva o projeto.
13. Tenta criar uma nova tarefa no projeto arquivado.
14. Exibe a regra de negócio impedindo a operação.

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

A persistência foi implementada em memória para manter o foco nos conceitos de Orientação a Objetos e Domain-Driven Design.

Em uma evolução futura, os repositories em memória poderiam ser substituídos por repositories com banco de dados, sem alterar as regras principais do domínio.
