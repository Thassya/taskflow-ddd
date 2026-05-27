using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Domain.Rules
{
    public class RegraTarefaNaoConcluida : IRegraConclusaoTarefa
    {
        public void Validar(Tarefa tarefa)
        {
            if (tarefa == null)
                throw new DomainException("Task is required.");

            if (tarefa.EstaConcluida())
                throw new DomainException("Completed tasks cannot be completed again.");
        }
    }
}
