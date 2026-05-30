using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Tasks;
using Xunit;

namespace TaskFlow.Tests.Tasks
{
    public class TarefaAdditionalTests
    {
        [Fact]
        public void AtribuirResponsavel_NaoDeveAceitarGuidVazio()
        {
            var tarefa = CriarTarefaValida();

            Assert.Throws<DomainException>(() =>
                tarefa.AtribuirResponsavel(Guid.Empty));
        }

        [Fact]
        public void Iniciar_NaoDevePermitirIniciarDuasVezes()
        {
            var tarefa = CriarTarefaValida();

            tarefa.Iniciar();

            Assert.Throws<DomainException>(() =>
                tarefa.Iniciar());
        }

        [Fact]
        public void AlterarTitulo_DeveAlterarQuandoTarefaNaoEstaConcluida()
        {
            var tarefa = CriarTarefaValida();

            tarefa.AlterarTitulo(new TituloTarefa("Novo titulo"));

            Assert.Equal("Novo titulo", tarefa.Titulo.Valor);
        }

        [Fact]
        public void AlterarTitulo_NaoDeveAceitarTituloNulo()
        {
            var tarefa = CriarTarefaValida();

            Assert.Throws<DomainException>(() =>
                tarefa.AlterarTitulo(null));
        }

        [Fact]
        public void AlterarDescricao_DeveAlterarQuandoTarefaNaoEstaConcluida()
        {
            var tarefa = CriarTarefaValida();

            tarefa.AlterarDescricao(new DescricaoTarefa("Nova descricao da tarefa."));

            Assert.Equal("Nova descricao da tarefa.", tarefa.Descricao.Valor);
        }

        [Fact]
        public void AlterarPrazo_DeveAlterarQuandoTarefaNaoEstaConcluida()
        {
            var tarefa = CriarTarefaValida();

            var novoPrazo = new Prazo(DateTime.Today.AddDays(10));

            tarefa.AlterarPrazo(novoPrazo);

            Assert.Equal(novoPrazo, tarefa.Prazo);
        }

        [Fact]
        public void AlterarDescricao_NaoDevePermitirTarefaConcluida()
        {
            var tarefa = CriarTarefaValida();

            tarefa.AtribuirResponsavel(Guid.NewGuid());
            tarefa.Iniciar();
            tarefa.Concluir();

            Assert.Throws<DomainException>(() =>
                tarefa.AlterarDescricao(new DescricaoTarefa("Tentativa invalida.")));
        }

        private static Tarefa CriarTarefaValida()
        {
            var factory = new TarefaFactory();

            return factory.Criar(
                Guid.NewGuid(),
                "Criar testes",
                "Criar testes unitarios do dominio.",
                Prioridade.Alta,
                DateTime.Today.AddDays(3));
        }
    }
}
