using System;
using System.Collections.Generic;
using TaskFlow.Application.Projetos;
using TaskFlow.Application.Tarefas;
using TaskFlow.Domain.Acl;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;
using TaskFlow.Domain.Rules;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.Tasks;
using TaskFlow.Infrastructure.Persistence;
using Xunit;

namespace TaskFlow.Tests.Application
{
    public class ApplicationServiceTests
    {
        [Fact]
        public void ProjetoApplicationService_DeveCriarProjetoESalvarNoRepositorio()
        {
            var repository = new InMemoryProjetoRepository();
            var service = new ProjetoApplicationService(
                repository,
                new ProjetoFactory());

            var projeto = service.CriarProjeto(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");

            var projetoSalvo = repository.ObterPorId(projeto.Id);

            Assert.NotNull(projetoSalvo);
            Assert.Equal(projeto.Id, projetoSalvo.Id);
        }

        [Fact]
        public void ProjetoApplicationService_DeveAdicionarMembro()
        {
            var repository = new InMemoryProjetoRepository();
            var service = new ProjetoApplicationService(
                repository,
                new ProjetoFactory());

            var projeto = service.CriarProjeto(
                "Projeto Teste",
                Guid.NewGuid(),
                "Gestor");

            var membroId = Guid.NewGuid();

            service.AdicionarMembro(
                projeto.Id,
                membroId,
                "Membro",
                false);

            Assert.True(projeto.UsuarioPertenceAoProjeto(membroId));
        }

        [Fact]
        public void TarefaApplicationService_NaoDeveCriarTarefaQuandoProjetoNaoEstaAtivo()
        {
            var service = CriarTarefaApplicationService(
                projetoAtivo: false,
                usuarioPertenceAoProjeto: true);

            Assert.Throws<DomainException>(() =>
                service.CriarTarefa(
                    Guid.NewGuid(),
                    "Tarefa Teste",
                    "Descricao da tarefa teste.",
                    Prioridade.Alta,
                    DateTime.Today.AddDays(5)));
        }

        [Fact]
        public void TarefaApplicationService_DeveExecutarFluxoCompletoDaTarefa()
        {
            var usuarioId = Guid.NewGuid();

            var service = CriarTarefaApplicationService(
                projetoAtivo: true,
                usuarioPertenceAoProjeto: true);

            var tarefa = service.CriarTarefa(
                Guid.NewGuid(),
                "Tarefa Teste",
                "Descricao da tarefa teste.",
                Prioridade.Alta,
                DateTime.Today.AddDays(5));

            service.AtribuirResponsavel(tarefa.Id, usuarioId);
            service.IniciarTarefa(tarefa.Id);
            service.ConcluirTarefa(tarefa.Id);

            Assert.Equal(StatusTarefa.Concluida, tarefa.Status);
            Assert.Equal(usuarioId, tarefa.ResponsavelId);
        }

        private static TarefaApplicationService CriarTarefaApplicationService(
            bool projetoAtivo,
            bool usuarioPertenceAoProjeto)
        {
            var tarefaRepository = new InMemoryTarefaRepository();

            var projetoAcl = new ProjetoAclFake(
                projetoAtivo,
                usuarioPertenceAoProjeto);

            var regrasConclusao = new List<IRegraConclusaoTarefa>
            {
                new RegraTarefaNaoConcluida(),
                new RegraTarefaComResponsavel(),
                new RegraTarefaEmAndamento()
            };

            return new TarefaApplicationService(
                tarefaRepository,
                new TarefaFactory(),
                projetoAcl,
                new AlocacaoTarefaService(projetoAcl),
                new ConclusaoTarefaService(regrasConclusao),
                new VerificadorPrazoService());
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
