using System;
using TaskFlow.Domain.Projects;
using TaskFlow.Domain.Tasks;
using Xunit;

namespace TaskFlow.Tests.Factories
{
    public class FactoryTests
    {
        [Fact]
        public void ProjetoFactory_DeveImplementarIProjetoFactory()
        {
            IProjetoFactory factory = new ProjetoFactory();

            var projeto = factory.Criar(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");

            Assert.NotEqual(Guid.Empty, projeto.Id);
            Assert.Equal("Projeto Teste", projeto.Nome.Valor);
            Assert.True(projeto.EstaAtivo());
            Assert.Single(projeto.Membros);
        }

        [Fact]
        public void TarefaFactory_DeveImplementarITarefaFactory()
        {
            ITarefaFactory factory = new TarefaFactory();

            var projetoId = Guid.NewGuid();

            var tarefa = factory.Criar(
                projetoId,
                "Tarefa Teste",
                "Descricao da tarefa teste.",
                Prioridade.Alta,
                DateTime.Today.AddDays(5));

            Assert.NotEqual(Guid.Empty, tarefa.Id);
            Assert.Equal(projetoId, tarefa.ProjetoId);
            Assert.Equal(StatusTarefa.Backlog, tarefa.Status);
            Assert.Null(tarefa.ResponsavelId);
        }
    }
}
