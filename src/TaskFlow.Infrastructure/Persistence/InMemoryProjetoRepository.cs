using System;
using System.Collections.Generic;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;

namespace TaskFlow.Infrastructure.Persistence
{
    public class InMemoryProjetoRepository : IProjetoRepository
    {
        private readonly Dictionary<Guid, Projeto> _projetos = new Dictionary<Guid, Projeto>();

        public Projeto ObterPorId(Guid id)
        {
            if (id == Guid.Empty)
                throw new DomainException("Project id cannot be empty.");

            if (!_projetos.ContainsKey(id))
                return null;

            return _projetos[id];
        }

        public void Adicionar(Projeto projeto)
        {
            if (projeto == null)
                throw new DomainException("Project is required.");

            if (_projetos.ContainsKey(projeto.Id))
                throw new DomainException("Project already exists.");

            _projetos.Add(projeto.Id, projeto);
        }

        public void Atualizar(Projeto projeto)
        {
            if (projeto == null)
                throw new DomainException("Project is required.");

            if (!_projetos.ContainsKey(projeto.Id))
                throw new DomainException("Project was not found.");

            _projetos[projeto.Id] = projeto;
        }
    }
}
