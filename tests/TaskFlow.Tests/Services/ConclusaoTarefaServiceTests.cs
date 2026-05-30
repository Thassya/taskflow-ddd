using System;
using System.Collections.Generic;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Rules;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.Tasks;
using Xunit;

namespace TaskFlow.Tests.Services
{
    public class ConclusaoTarefaServiceTests
    {
        [Fact]
        public void Concluir_DeveConcluirTarefaQuandoTodasAsRegrasSaoAtendidas()
        {
            var tarefa = CriarTarefaValida();

            tarefa.AtribuirResponsavel(Guid.NewGuid());
            tarefa.Iniciar();

            var service = CriarService();

            service.Concluir(tarefa);

            Assert.Equal(StatusTarefa.Concluida, tarefa.Status);
        }

        [Fact]
        public void Concluir_NaoDevePermitirTarefaSemResponsavel()
        {
            var tarefa = CriarTarefaValida();

            tarefa.Iniciar();

            var service = CriarService();

            Assert.Throws<DomainException>(() =>
                service.Concluir(tarefa));
        }

        [Fact]
        public void Concluir_NaoDevePermitirTarefaEmBacklog()
        {
            var tarefa = CriarTarefaValida();

            tarefa.AtribuirResponsavel(Guid.NewGuid());

            var service = CriarService();

            Assert.Throws<DomainException>(() =>
                service.Concluir(tarefa));
        }

        private static ConclusaoTarefaService CriarService()
        {
            var regras = new List<IRegraConclusaoTarefa>
            {
                new RegraTarefaNaoConcluida(),
                new RegraTarefaComResponsavel(),
                new RegraTarefaEmAndamento()
            };

            return new ConclusaoTarefaService(regras);
        }

        private static Tarefa CriarTarefaValida()
        {
            var factory = new TarefaFactory();

            return factory.Criar(
                Guid.NewGuid(),
                "Tarefa Teste",
                "Descricao da tarefa teste.",
                Prioridade.Alta,
                DateTime.Today.AddDays(5));
        }
    }
}
