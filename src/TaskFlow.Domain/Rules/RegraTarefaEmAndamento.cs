using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Domain.Rules
{
    public class RegraTarefaEmAndamento : IRegraConclusaoTarefa
    {
        public void Validar(Tarefa tarefa)
        {
            if (tarefa == null)
                throw new DomainException("Task is required.");

            if (tarefa.Status != StatusTarefa.EmAndamento)
                throw new DomainException("Only tasks in progress can be completed.");
        }
    }
}
