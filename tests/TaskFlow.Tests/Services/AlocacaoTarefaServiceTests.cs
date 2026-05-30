using System;
using TaskFlow.Domain.Acl;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.Tasks;
using Xunit;

namespace TaskFlow.Tests.Services
{
    public class AlocacaoTarefaServiceTests
    {
        [Fact]
        public void AtribuirResponsavel_DevePermitirQuandoUsuarioPertenceAoProjeto()
        {
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var tarefa = CriarTarefa(projetoId);

            var acl = new ProjetoAclFake(
                projetoAtivo: true,
                usuarioPertenceAoProjeto: true);

            var service = new AlocacaoTarefaService(acl);

            service.AtribuirResponsavel(tarefa, usuarioId);

            Assert.Equal(usuarioId, tarefa.ResponsavelId);
        }

        [Fact]
        public void AtribuirResponsavel_NaoDevePermitirQuandoProjetoEstaArquivado()
        {
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var tarefa = CriarTarefa(projetoId);

            var acl = new ProjetoAclFake(
                projetoAtivo: false,
                usuarioPertenceAoProjeto: true);

            var service = new AlocacaoTarefaService(acl);

            Assert.Throws<DomainException>(() =>
                service.AtribuirResponsavel(tarefa, usuarioId));
        }

        [Fact]
        public void AtribuirResponsavel_NaoDevePermitirQuandoUsuarioNaoPertenceAoProjeto()
        {
            var projetoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var tarefa = CriarTarefa(projetoId);

            var acl = new ProjetoAclFake(
                projetoAtivo: true,
                usuarioPertenceAoProjeto: false);

            var service = new AlocacaoTarefaService(acl);

            Assert.Throws<DomainException>(() =>
                service.AtribuirResponsavel(tarefa, usuarioId));
        }

        private static Tarefa CriarTarefa(Guid projetoId)
        {
            var factory = new TarefaFactory();

            return factory.Criar(
                projetoId,
                "Criar testes",
                "Criar testes unitários do domínio.",
                Prioridade.Alta,
                DateTime.Today.AddDays(3));
        }

        private class ProjetoAclFake : IProjetoAcl
        {
            private readonly bool _projetoAtivo;
            private readonly bool _usuarioPertenceAoProjeto;

            public ProjetoAclFake(
                bool projetoAtivo,
                bool usuarioPertenceAoProjeto)
            {
                _projetoAtivo = projetoAtivo;
                _usuarioPertenceAoProjeto = usuarioPertenceAoProjeto;
            }

            public bool ProjetoEstaAtivo(Guid projetoId)
            {
                return _projetoAtivo;
            }

            public bool UsuarioPertenceAoProjeto(Guid projetoId, Guid usuarioId)
            {
                return _usuarioPertenceAoProjeto;
            }
        }
    }
}
