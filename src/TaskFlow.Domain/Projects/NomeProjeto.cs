using System.Collections.Generic;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Projects
{
    public class NomeProjeto : ValueObject
    {
        public string Valor { get; private set; }

        public NomeProjeto(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("Project name cannot be empty.");

            if (valor.Length < 3)
                throw new DomainException("Project name must have at least 3 characters.");

            if (valor.Length > 100)
                throw new DomainException("Project name cannot have more than 100 characters.");

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
