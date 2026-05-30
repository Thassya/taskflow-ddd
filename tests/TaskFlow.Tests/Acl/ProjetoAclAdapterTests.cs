using System;
using TaskFlow.Domain.Projects;
using TaskFlow.Infrastructure.Acl;
using TaskFlow.Infrastructure.Persistence;
using Xunit;

namespace TaskFlow.Tests.Acl
{
    public class ProjetoAclAdapterTests
    {
        [Fact]
        public void ProjetoEstaAtivo_DeveRetornarTrueQuandoProjetoExisteEEstáAtivo()
        {
            var repository = new InMemoryProjetoRepository();
            var projeto = CriarProjeto();

            repository.Adicionar(projeto);

            var adapter = new ProjetoAclAdapter(repository);

            var resultado = adapter.ProjetoEstaAtivo(projeto.Id);

            Assert.True(resultado);
        }

        [Fact]
        public void ProjetoEstaAtivo_DeveRetornarFalseQuandoProjetoNaoExiste()
        {
            var repository = new InMemoryProjetoRepository();
            var adapter = new ProjetoAclAdapter(repository);

            var resultado = adapter.ProjetoEstaAtivo(Guid.NewGuid());

            Assert.False(resultado);
        }

        [Fact]
        public void ProjetoEstaAtivo_DeveRetornarFalseQuandoProjetoEstaArquivado()
        {
            var repository = new InMemoryProjetoRepository();
            var projeto = CriarProjeto();

            projeto.Arquivar();
            repository.Adicionar(projeto);

            var adapter = new ProjetoAclAdapter(repository);

            var resultado = adapter.ProjetoEstaAtivo(projeto.Id);

            Assert.False(resultado);
        }

        [Fact]
        public void UsuarioPertenceAoProjeto_DeveRetornarTrueQuandoUsuarioEhMembro()
        {
            var repository = new InMemoryProjetoRepository();
            var projeto = CriarProjeto();
            var usuarioId = Guid.NewGuid();

            projeto.AdicionarMembro(usuarioId, "Membro", false);
            repository.Adicionar(projeto);

            var adapter = new ProjetoAclAdapter(repository);

            var resultado = adapter.UsuarioPertenceAoProjeto(projeto.Id, usuarioId);

            Assert.True(resultado);
        }

        [Fact]
        public void UsuarioPertenceAoProjeto_DeveRetornarFalseQuandoProjetoNaoExiste()
        {
            var repository = new InMemoryProjetoRepository();
            var adapter = new ProjetoAclAdapter(repository);

            var resultado = adapter.UsuarioPertenceAoProjeto(Guid.NewGuid(), Guid.NewGuid());

            Assert.False(resultado);
        }

        [Fact]
        public void UsuarioPertenceAoProjeto_DeveRetornarFalseQuandoUsuarioNaoEhMembro()
        {
            var repository = new InMemoryProjetoRepository();
            var projeto = CriarProjeto();

            repository.Adicionar(projeto);

            var adapter = new ProjetoAclAdapter(repository);

            var resultado = adapter.UsuarioPertenceAoProjeto(projeto.Id, Guid.NewGuid());

            Assert.False(resultado);
        }

        private static Projeto CriarProjeto()
        {
            var factory = new ProjetoFactory();

            return factory.Criar(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");
        }
    }
}
