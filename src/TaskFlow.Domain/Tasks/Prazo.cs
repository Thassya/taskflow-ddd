using System;
using System.Collections.Generic;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Tasks
{
    public class Prazo : ValueObject
    {
        public DateTime DataLimite { get; private set; }

        public Prazo(DateTime dataLimite)
        {
            if (dataLimite == DateTime.MinValue)
                throw new DomainException("Task due date is invalid.");

            DataLimite = dataLimite.Date;
        }

        public bool EstaVencido(DateTime dataAtual)
        {
            return dataAtual.Date > DataLimite;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DataLimite;
        }

        public override string ToString()
        {
            return DataLimite.ToString("yyyy-MM-dd");
        }
    }
}
