using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Domain.Rules
{
    public class RegraTarefaComResponsavel : IRegraConclusaoTarefa
    {
        public void Validar(Tarefa tarefa)
        {
            if (tarefa == null)
                throw new DomainException("Task is required.");

            if (!tarefa.ResponsavelId.HasValue)
                throw new DomainException("A task cannot be completed without an assignee.");
        }
    }
}
