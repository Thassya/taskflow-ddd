using System;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.Tasks;
using Xunit;

namespace TaskFlow.Tests.Services
{
    public class VerificadorPrazoServiceTests
    {
        [Fact]
        public void EstaAtrasada_DeveRetornarTrueQuandoPrazoVenceuETarefaNaoConcluida()
        {
            var factory = new TarefaFactory();

            var tarefa = factory.Criar(
                Guid.NewGuid(),
                "Tarefa atrasada",
                "Tarefa usada para teste de atraso.",
                Prioridade.Media,
                DateTime.Today.AddDays(-1));

            var service = new VerificadorPrazoService();

            var resultado = service.EstaAtrasada(tarefa, DateTime.Today);

            Assert.True(resultado);
        }

        [Fact]
        public void EstaAtrasada_DeveRetornarFalseQuandoTarefaEstaConcluida()
        {
            var factory = new TarefaFactory();

            var tarefa = factory.Criar(
                Guid.NewGuid(),
                "Tarefa concluída",
                "Tarefa usada para teste de atraso.",
                Prioridade.Media,
                DateTime.Today.AddDays(-1));

            tarefa.AtribuirResponsavel(Guid.NewGuid());
            tarefa.Iniciar();
            tarefa.Concluir();

            var service = new VerificadorPrazoService();

            var resultado = service.EstaAtrasada(tarefa, DateTime.Today);

            Assert.False(resultado);
        }
    }
}
