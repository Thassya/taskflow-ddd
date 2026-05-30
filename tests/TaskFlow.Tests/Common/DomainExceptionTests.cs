using System;
using TaskFlow.Domain.Exceptions;
using Xunit;

namespace TaskFlow.Tests.Common
{
    public class DomainExceptionTests
    {
        [Fact]
        public void DomainException_DeveArmazenarMensagem()
        {
            var exception = new DomainException("Erro de dominio.");

            Assert.Equal("Erro de dominio.", exception.Message);
        }

        [Fact]
        public void DomainException_DeveArmazenarInnerException()
        {
            var innerException = new InvalidOperationException("Erro interno.");

            var exception = new DomainException(
                "Erro de dominio.",
                innerException);

            Assert.Equal("Erro de dominio.", exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
