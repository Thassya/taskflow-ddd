using System.Collections.Generic;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Tasks
{
    public class TituloTarefa : ValueObject
    {
        public string Valor { get; private set; }

        public TituloTarefa(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("Task title cannot be empty.");

            if (valor.Length < 3)
                throw new DomainException("Task title must have at least 3 characters.");

            if (valor.Length > 100)
                throw new DomainException("Task title cannot have more than 100 characters.");

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
