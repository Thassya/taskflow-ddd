using System;
using System.Collections.Generic;

namespace TaskFlow.Domain.Tasks
{
    public interface ITarefaRepository
    {
        Tarefa ObterPorId(Guid id);
        IEnumerable<Tarefa> ObterPorProjeto(Guid projetoId);
        void Adicionar(Tarefa tarefa);
        void Atualizar(Tarefa tarefa);
    }
}
