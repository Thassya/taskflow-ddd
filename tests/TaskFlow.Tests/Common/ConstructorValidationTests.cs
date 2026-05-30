using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;
using TaskFlow.Domain.Tasks;
using Xunit;

namespace TaskFlow.Tests.Common
{
    public class ConstructorValidationTests
    {
        [Fact]
        public void MembroProjeto_NaoDeveAceitarUsuarioIdVazio()
        {
            Assert.Throws<DomainException>(() =>
                new MembroProjeto(
                    Guid.NewGuid(),
                    Guid.Empty,
                    "Membro",
                    false));
        }

        [Fact]
        public void MembroProjeto_NaoDeveAceitarNomeVazio()
        {
            Assert.Throws<DomainException>(() =>
                new MembroProjeto(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "",
                    false));
        }

        [Fact]
        public void Projeto_NaoDeveAceitarNomeNulo()
        {
            var membro = new MembroProjeto(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Gestor",
                true);

            Assert.Throws<DomainException>(() =>
                new Projeto(
                    Guid.NewGuid(),
                    null,
                    membro));
        }

        [Fact]
        public void Projeto_NaoDeveAceitarMembroResponsavelNulo()
        {
            Assert.Throws<DomainException>(() =>
                new Projeto(
                    Guid.NewGuid(),
                    new NomeProjeto("Projeto Teste"),
                    null));
        }

        [Fact]
        public void Projeto_NaoDeveAceitarMembroInicialSemResponsabilidade()
        {
            var membro = new MembroProjeto(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Membro",
                false);

            Assert.Throws<DomainException>(() =>
                new Projeto(
                    Guid.NewGuid(),
                    new NomeProjeto("Projeto Teste"),
                    membro));
        }

        [Fact]
        public void Tarefa_NaoDeveAceitarProjetoIdVazio()
        {
            Assert.Throws<DomainException>(() =>
                new Tarefa(
                    Guid.NewGuid(),
                    Guid.Empty,
                    new TituloTarefa("Titulo Teste"),
                    new DescricaoTarefa("Descricao teste."),
                    Prioridade.Media,
                    new Prazo(DateTime.Today.AddDays(5))));
        }

        [Fact]
        public void Tarefa_NaoDeveAceitarTituloNulo()
        {
            Assert.Throws<DomainException>(() =>
                new Tarefa(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    null,
                    new DescricaoTarefa("Descricao teste."),
                    Prioridade.Media,
                    new Prazo(DateTime.Today.AddDays(5))));
        }

        [Fact]
        public void Tarefa_NaoDeveAceitarDescricaoNula()
        {
            Assert.Throws<DomainException>(() =>
                new Tarefa(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    new TituloTarefa("Titulo Teste"),
                    null,
                    Prioridade.Media,
                    new Prazo(DateTime.Today.AddDays(5))));
        }

        [Fact]
        public void Tarefa_NaoDeveAceitarPrazoNulo()
        {
            Assert.Throws<DomainException>(() =>
                new Tarefa(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    new TituloTarefa("Titulo Teste"),
                    new DescricaoTarefa("Descricao teste."),
                    Prioridade.Media,
                    null));
        }
    }
}
