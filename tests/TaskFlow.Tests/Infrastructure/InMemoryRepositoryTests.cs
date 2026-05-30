using System;
using System.Linq;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;
using TaskFlow.Domain.Tasks;
using TaskFlow.Infrastructure.Persistence;
using Xunit;

namespace TaskFlow.Tests.Infrastructure
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public void ProjetoRepository_DeveAdicionarEObterProjetoPorId()
        {
            var repository = new InMemoryProjetoRepository();
            var projeto = CriarProjeto();

            repository.Adicionar(projeto);

            var resultado = repository.ObterPorId(projeto.Id);

            Assert.Equal(projeto, resultado);
        }

        [Fact]
        public void ProjetoRepository_ObterPorId_DeveRetornarNullQuandoNaoEncontrar()
        {
            var repository = new InMemoryProjetoRepository();

            var resultado = repository.ObterPorId(Guid.NewGuid());

            Assert.Null(resultado);
        }

        [Fact]
        public void ProjetoRepository_NaoDeveAdicionarProjetoDuplicado()
        {
            var repository = new InMemoryProjetoRepository();
            var projeto = CriarProjeto();

            repository.Adicionar(projeto);

            Assert.Throws<DomainException>(() =>
                repository.Adicionar(projeto));
        }

        [Fact]
        public void ProjetoRepository_NaoDeveAtualizarProjetoInexistente()
        {
            var repository = new InMemoryProjetoRepository();
            var projeto = CriarProjeto();

            Assert.Throws<DomainException>(() =>
                repository.Atualizar(projeto));
        }

        [Fact]
        public void TarefaRepository_DeveAdicionarEObterTarefaPorId()
        {
            var repository = new InMemoryTarefaRepository();
            var tarefa = CriarTarefa(Guid.NewGuid());

            repository.Adicionar(tarefa);

            var resultado = repository.ObterPorId(tarefa.Id);

            Assert.Equal(tarefa, resultado);
        }

        [Fact]
        public void TarefaRepository_DeveObterTarefasPorProjeto()
        {
            var repository = new InMemoryTarefaRepository();

            var projetoId = Guid.NewGuid();
            var outroProjetoId = Guid.NewGuid();

            var tarefa1 = CriarTarefa(projetoId);
            var tarefa2 = CriarTarefa(projetoId);
            var tarefaOutroProjeto = CriarTarefa(outroProjetoId);

            repository.Adicionar(tarefa1);
            repository.Adicionar(tarefa2);
            repository.Adicionar(tarefaOutroProjeto);

            var resultado = repository.ObterPorProjeto(projetoId).ToList();

            Assert.Equal(2, resultado.Count);
            Assert.Contains(tarefa1, resultado);
            Assert.Contains(tarefa2, resultado);
            Assert.DoesNotContain(tarefaOutroProjeto, resultado);
        }

        [Fact]
        public void TarefaRepository_NaoDeveAdicionarTarefaDuplicada()
        {
            var repository = new InMemoryTarefaRepository();
            var tarefa = CriarTarefa(Guid.NewGuid());

            repository.Adicionar(tarefa);

            Assert.Throws<DomainException>(() =>
                repository.Adicionar(tarefa));
        }

        [Fact]
        public void TarefaRepository_NaoDeveAtualizarTarefaInexistente()
        {
            var repository = new InMemoryTarefaRepository();
            var tarefa = CriarTarefa(Guid.NewGuid());

            Assert.Throws<DomainException>(() =>
                repository.Atualizar(tarefa));
        }

        private static Projeto CriarProjeto()
        {
            var factory = new ProjetoFactory();

            return factory.Criar(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");
        }

        private static Tarefa CriarTarefa(Guid projetoId)
        {
            var factory = new TarefaFactory();

            return factory.Criar(
                projetoId,
                "Tarefa Teste",
                "Descricao da tarefa teste.",
                Prioridade.Media,
                DateTime.Today.AddDays(5));
        }
    }
}
