using TaskFlow.Domain.Tasks;

namespace TaskFlow.Domain.Rules
{
    public interface IRegraConclusaoTarefa
    {
        void Validar(Tarefa tarefa);
    }
}
