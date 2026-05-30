using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;
using Xunit;

namespace TaskFlow.Tests.Projects
{
    public class ProjetoTests
    {
        [Fact]
        public void CriarProjeto_DeveIniciarComStatusAtivo()
        {
            var factory = new ProjetoFactory();

            var projeto = factory.Criar(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");

            Assert.Equal(StatusProjeto.Ativo, projeto.Status);
        }

        [Fact]
        public void AdicionarMembro_DeveAdicionarUsuarioAoProjeto()
        {
            var factory = new ProjetoFactory();
            var usuarioId = Guid.NewGuid();

            var projeto = factory.Criar(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");

            projeto.AdicionarMembro(
                usuarioId,
                "Membro",
                false);

            Assert.True(projeto.UsuarioPertenceAoProjeto(usuarioId));
        }

        [Fact]
        public void RemoverMembro_NaoDeveRemoverUltimoResponsavel()
        {
            var factory = new ProjetoFactory();

            var gestorId = Guid.NewGuid();

            var projeto = factory.Criar(
                "Projeto Teste",
                gestorId,
                "Gestor");

            Assert.Throws<DomainException>(() =>
                projeto.RemoverMembro(gestorId));
        }

        [Fact]
        public void AdicionarMembro_NaoDevePermitirProjetoArquivado()
        {
            var factory = new ProjetoFactory();

            var projeto = factory.Criar(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");

            projeto.Arquivar();

            Assert.Throws<DomainException>(() =>
                projeto.AdicionarMembro(
                    Guid.NewGuid(),
                    "Novo Membro",
                    false));
        }
    }
}
