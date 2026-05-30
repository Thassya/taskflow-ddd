using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;
using Xunit;

namespace TaskFlow.Tests.Projects
{
    public class ProjetoAdditionalTests
    {
        [Fact]
        public void AdicionarMembro_NaoDevePermitirUsuarioDuplicado()
        {
            var projeto = CriarProjetoValido();
            var usuarioId = Guid.NewGuid();

            projeto.AdicionarMembro(usuarioId, "Membro", false);

            Assert.Throws<DomainException>(() =>
                projeto.AdicionarMembro(usuarioId, "Membro Duplicado", false));
        }

        [Fact]
        public void RemoverMembro_DevePermitirRemoverResponsavelQuandoExisteOutroResponsavel()
        {
            var gestorId = Guid.NewGuid();
            var segundoResponsavelId = Guid.NewGuid();

            var factory = new ProjetoFactory();

            var projeto = factory.Criar(
                "Projeto Teste",
                gestorId,
                "Gestor");

            projeto.AdicionarMembro(
                segundoResponsavelId,
                "Segundo Responsavel",
                true);

            projeto.RemoverMembro(gestorId);

            Assert.False(projeto.UsuarioPertenceAoProjeto(gestorId));
            Assert.True(projeto.UsuarioPertenceAoProjeto(segundoResponsavelId));
        }

        [Fact]
        public void RemoverMembro_NaoDevePermitirUsuarioInexistente()
        {
            var projeto = CriarProjetoValido();

            Assert.Throws<DomainException>(() =>
                projeto.RemoverMembro(Guid.NewGuid()));
        }

        [Fact]
        public void Arquivar_NaoDevePermitirArquivarDuasVezes()
        {
            var projeto = CriarProjetoValido();

            projeto.Arquivar();

            Assert.Throws<DomainException>(() =>
                projeto.Arquivar());
        }

        [Fact]
        public void AlterarNome_DeveAlterarNomeQuandoProjetoEstaAtivo()
        {
            var projeto = CriarProjetoValido();

            projeto.AlterarNome(new NomeProjeto("Novo Nome Projeto"));

            Assert.Equal("Novo Nome Projeto", projeto.Nome.Valor);
        }

        [Fact]
        public void AlterarNome_NaoDevePermitirProjetoArquivado()
        {
            var projeto = CriarProjetoValido();

            projeto.Arquivar();

            Assert.Throws<DomainException>(() =>
                projeto.AlterarNome(new NomeProjeto("Novo Nome Projeto")));
        }

        private static Projeto CriarProjetoValido()
        {
            var factory = new ProjetoFactory();

            return factory.Criar(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");
        }
    }
}
