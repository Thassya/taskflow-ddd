using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;
using TaskFlow.Domain.Tasks;
using Xunit;

namespace TaskFlow.Tests.Common
{
    public class ValueObjectsTests
    {
        [Fact]
        public void NomeProjeto_DeveRemoverEspacos()
        {
            var nome = new NomeProjeto("  Projeto Teste  ");

            Assert.Equal("Projeto Teste", nome.Valor);
        }

        [Fact]
        public void NomeProjeto_NaoDeveAceitarNomeCurto()
        {
            Assert.Throws<DomainException>(() =>
                new NomeProjeto("AB"));
        }

        [Fact]
        public void NomeProjeto_ComMesmoValor_DeveSerIgual()
        {
            var nome1 = new NomeProjeto("Projeto Teste");
            var nome2 = new NomeProjeto("Projeto Teste");

            Assert.Equal(nome1, nome2);
        }

        [Fact]
        public void TituloTarefa_NaoDeveAceitarValorVazio()
        {
            Assert.Throws<DomainException>(() =>
                new TituloTarefa(""));
        }

        [Fact]
        public void DescricaoTarefa_NaoDeveAceitarMaisDe500Caracteres()
        {
            var descricaoGrande = new string('A', 501);

            Assert.Throws<DomainException>(() =>
                new DescricaoTarefa(descricaoGrande));
        }

        [Fact]
        public void Prazo_NaoDeveAceitarDataMinima()
        {
            Assert.Throws<DomainException>(() =>
                new Prazo(DateTime.MinValue));
        }

        [Fact]
        public void Prazo_DeveIdentificarQuandoEstaVencido()
        {
            var prazo = new Prazo(DateTime.Today.AddDays(-1));

            var resultado = prazo.EstaVencido(DateTime.Today);

            Assert.True(resultado);
        }

        [Fact]
        public void Prazo_NaoDeveEstarVencidoQuandoDataAtualIgualDataLimite()
        {
            var prazo = new Prazo(DateTime.Today);

            var resultado = prazo.EstaVencido(DateTime.Today);

            Assert.False(resultado);
        }
    }
}
