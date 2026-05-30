using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;
using TaskFlow.Domain.Tasks;
using TaskFlow.Infrastructure.Persistence;
using Xunit;

namespace TaskFlow.Tests.Infrastructure
{
    public class InMemoryRepositoryValidationTests
    {
        [Fact]
        public void ProjetoRepository_ObterPorId_NaoDeveAceitarGuidVazio()
        {
            var repository = new InMemoryProjetoRepository();

            Assert.Throws<DomainException>(() =>
                repository.ObterPorId(Guid.Empty));
        }

        [Fact]
        public void ProjetoRepository_Adicionar_NaoDeveAceitarProjetoNulo()
        {
            var repository = new InMemoryProjetoRepository();

            Assert.Throws<DomainException>(() =>
                repository.Adicionar(null));
        }

        [Fact]
        public void ProjetoRepository_Atualizar_NaoDeveAceitarProjetoNulo()
        {
            var repository = new InMemoryProjetoRepository();

            Assert.Throws<DomainException>(() =>
                repository.Atualizar(null));
        }

        [Fact]
        public void TarefaRepository_ObterPorId_NaoDeveAceitarGuidVazio()
        {
            var repository = new InMemoryTarefaRepository();

            Assert.Throws<DomainException>(() =>
                repository.ObterPorId(Guid.Empty));
        }

        [Fact]
        public void TarefaRepository_ObterPorProjeto_NaoDeveAceitarGuidVazio()
        {
            var repository = new InMemoryTarefaRepository();

            Assert.Throws<DomainException>(() =>
                repository.ObterPorProjeto(Guid.Empty));
        }

        [Fact]
        public void TarefaRepository_Adicionar_NaoDeveAceitarTarefaNula()
        {
            var repository = new InMemoryTarefaRepository();

            Assert.Throws<DomainException>(() =>
                repository.Adicionar(null));
        }

        [Fact]
        public void TarefaRepository_Atualizar_NaoDeveAceitarTarefaNula()
        {
            var repository = new InMemoryTarefaRepository();

            Assert.Throws<DomainException>(() =>
                repository.Atualizar(null));
        }
    }
}
