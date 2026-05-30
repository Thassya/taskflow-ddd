using System;

namespace TaskFlow.Domain.Tasks
{
    public interface ITarefaFactory
    {
        Tarefa Criar(
            Guid projetoId,
            string titulo,
            string descricao,
            Prioridade prioridade,
            DateTime dataLimite);
    }
}
