using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Tasks;
using Xunit;

namespace TaskFlow.Tests.Tasks
{
    public class TarefaTests
    {
        [Fact]
        public void CriarTarefa_DeveIniciarComStatusBacklog()
        {
            var factory = new TarefaFactory();

            var tarefa = factory.Criar(
                Guid.NewGuid(),
                "Criar testes",
                "Criar testes unitários do domínio.",
                Prioridade.Alta,
                DateTime.Today.AddDays(3));

            Assert.Equal(StatusTarefa.Backlog, tarefa.Status);
        }

        [Fact]
        public void Concluir_NaoDevePermitirTarefaSemResponsavel()
        {
            var tarefa = CriarTarefaValida();

            tarefa.Iniciar();

            Assert.Throws<DomainException>(() => tarefa.Concluir());
        }

        [Fact]
        public void Concluir_NaoDevePermitirTarefaEmBacklog()
        {
            var tarefa = CriarTarefaValida();

            tarefa.AtribuirResponsavel(Guid.NewGuid());

            Assert.Throws<DomainException>(() => tarefa.Concluir());
        }

        [Fact]
        public void Concluir_DeveAlterarStatusParaConcluida()
        {
            var tarefa = CriarTarefaValida();

            tarefa.AtribuirResponsavel(Guid.NewGuid());
            tarefa.Iniciar();

            tarefa.Concluir();

            Assert.Equal(StatusTarefa.Concluida, tarefa.Status);
        }

        [Fact]
        public void AlterarPrioridade_NaoDevePermitirTarefaConcluida()
        {
            var tarefa = CriarTarefaValida();

            tarefa.AtribuirResponsavel(Guid.NewGuid());
            tarefa.Iniciar();
            tarefa.Concluir();

            Assert.Throws<DomainException>(() =>
                tarefa.AlterarPrioridade(Prioridade.Critica));
        }

        private static Tarefa CriarTarefaValida()
        {
            var factory = new TarefaFactory();

            return factory.Criar(
                Guid.NewGuid(),
                "Criar testes",
                "Criar testes unitários do domínio.",
                Prioridade.Alta,
                DateTime.Today.AddDays(3));
        }
    }
}
