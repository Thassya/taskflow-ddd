using System;
using System.Collections.Generic;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Rules;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Domain.Services
{
    public class ConclusaoTarefaService
    {
        private readonly IReadOnlyCollection<IRegraConclusaoTarefa> _regras;

        public ConclusaoTarefaService(IEnumerable<IRegraConclusaoTarefa> regras)
        {
            if (regras == null)
                throw new ArgumentNullException(nameof(regras));

            _regras = new List<IRegraConclusaoTarefa>(regras);
        }

        public void Concluir(Tarefa tarefa)
        {
            if (tarefa == null)
                throw new DomainException("Task is required.");

            foreach (var regra in _regras)
            {
                regra.Validar(tarefa);
            }

            tarefa.Concluir();
        }
    }
}
