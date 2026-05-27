using System.Collections.Generic;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Tasks
{
    public class DescricaoTarefa : ValueObject
    {
        public string Valor { get; private set; }

        public DescricaoTarefa(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("Task description cannot be empty.");

            if (valor.Length > 500)
                throw new DomainException("Task description cannot have more than 500 characters.");

            Valor = valor.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString()
        {
            return Valor;
        }
    }
}
