using System;
using System.Collections.Generic;
using TaskFlow.Application.Tarefas;
using TaskFlow.Domain.Acl;
using TaskFlow.Domain.Projects;
using TaskFlow.Domain.Rules;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.Tasks;
using TaskFlow.Infrastructure.Acl;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("TaskFlow - DDD Console Application");
                Console.WriteLine("----------------------------------");

                var projetoRepository = new InMemoryProjetoRepository();
                var tarefaRepository = new InMemoryTarefaRepository();

                var projetoFactory = new ProjetoFactory();
                var tarefaFactory = new TarefaFactory();

                var usuarioGestorId = Guid.NewGuid();
                var usuarioMembroId = Guid.NewGuid();

                var projeto = projetoFactory.Criar(
                    "Projeto TaskFlow",
                    usuarioGestorId,
                    "Gestor do Projeto");

                projeto.AdicionarMembro(
                    usuarioMembroId,
                    "Membro da Equipe",
                    false);

                projetoRepository.Adicionar(projeto);

                IProjetoAcl projetoAcl = new ProjetoAclAdapter(projetoRepository);

                var alocacaoTarefaService = new AlocacaoTarefaService(projetoAcl);

                var regrasConclusao = new List<IRegraConclusaoTarefa>
                {
                    new RegraTarefaNaoConcluida(),
                    new RegraTarefaComResponsavel()
                };

                var conclusaoTarefaService = new ConclusaoTarefaService(regrasConclusao);
                var verificadorPrazoService = new VerificadorPrazoService();

                var tarefaApplicationService = new TarefaApplicationService(
                    tarefaRepository,
                    tarefaFactory,
                    projetoAcl,
                    alocacaoTarefaService,
                    conclusaoTarefaService,
                    verificadorPrazoService);

                var tarefa = tarefaApplicationService.CriarTarefa(
                    projeto.Id,
                    "Implementar domínio",
                    "Criar entidades, value objects, repositories, services e factories.",
                    Prioridade.Alta,
                    DateTime.Today.AddDays(3));

                Console.WriteLine();
                Console.WriteLine("Project created:");
                Console.WriteLine("Id: " + projeto.Id);
                Console.WriteLine("Name: " + projeto.Nome);
                Console.WriteLine("Status: " + projeto.Status);

                Console.WriteLine();
                Console.WriteLine("Task created:");
                Console.WriteLine("Id: " + tarefa.Id);
                Console.WriteLine("Title: " + tarefa.Titulo);
                Console.WriteLine("Priority: " + tarefa.Prioridade);
                Console.WriteLine("Status: " + tarefa.Status);
                Console.WriteLine("Due date: " + tarefa.Prazo);

                tarefaApplicationService.AtribuirResponsavel(
                    tarefa.Id,
                    usuarioMembroId);

                tarefaApplicationService.IniciarTarefa(tarefa.Id);

                var estaAtrasada = tarefaApplicationService.TarefaEstaAtrasada(
                    tarefa.Id,
                    DateTime.Today);

                Console.WriteLine();
                Console.WriteLine("Task after assignment and start:");
                Console.WriteLine("Responsible user id: " + tarefa.ResponsavelId);
                Console.WriteLine("Status: " + tarefa.Status);
                Console.WriteLine("Is overdue: " + estaAtrasada);

                tarefaApplicationService.ConcluirTarefa(tarefa.Id);

                Console.WriteLine();
                Console.WriteLine("Task completed:");
                Console.WriteLine("Status: " + tarefa.Status);

                Console.WriteLine();
                Console.WriteLine("Trying to create a task in an archived project...");

                projeto.Arquivar();
                projetoRepository.Atualizar(projeto);

                try
                {
                    tarefaApplicationService.CriarTarefa(
                        projeto.Id,
                        "Nova tarefa inválida",
                        "Esta tarefa não deve ser criada porque o projeto está arquivado.",
                        Prioridade.Media,
                        DateTime.Today.AddDays(5));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Business rule applied: " + ex.Message);
                }

                Console.WriteLine();
                Console.WriteLine("Application finished successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Unexpected error:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
