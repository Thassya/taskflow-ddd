using System;
using System.Collections.Generic;
using System.Linq;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Infrastructure.Persistence
{
    public class InMemoryTarefaRepository : ITarefaRepository
    {
        private readonly Dictionary<Guid, Tarefa> _tarefas = new Dictionary<Guid, Tarefa>();

        public Tarefa ObterPorId(Guid id)
        {
            if (id == Guid.Empty)
                throw new DomainException("Task id cannot be empty.");

            if (!_tarefas.ContainsKey(id))
                return null;

            return _tarefas[id];
        }

        public IEnumerable<Tarefa> ObterPorProjeto(Guid projetoId)
        {
            if (projetoId == Guid.Empty)
                throw new DomainException("Project id cannot be empty.");

            return _tarefas.Values
                .Where(tarefa => tarefa.ProjetoId == projetoId)
                .ToList();
        }

        public void Adicionar(Tarefa tarefa)
        {
            if (tarefa == null)
                throw new DomainException("Task is required.");

            if (_tarefas.ContainsKey(tarefa.Id))
                throw new DomainException("Task already exists.");

            _tarefas.Add(tarefa.Id, tarefa);
        }

        public void Atualizar(Tarefa tarefa)
        {
            if (tarefa == null)
                throw new DomainException("Task is required.");

            if (!_tarefas.ContainsKey(tarefa.Id))
                throw new DomainException("Task was not found.");

            _tarefas[tarefa.Id] = tarefa;
        }
    }
}
