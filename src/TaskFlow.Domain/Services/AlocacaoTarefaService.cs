using System;
using TaskFlow.Domain.Acl;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Domain.Services
{
    public class AlocacaoTarefaService
    {
        private readonly IProjetoAcl _projetoAcl;

        public AlocacaoTarefaService(IProjetoAcl projetoAcl)
        {
            if (projetoAcl == null)
                throw new ArgumentNullException(nameof(projetoAcl));

            _projetoAcl = projetoAcl;
        }

        public void AtribuirResponsavel(Tarefa tarefa, Guid usuarioId)
        {
            if (tarefa == null)
                throw new DomainException("Task is required.");

            if (usuarioId == Guid.Empty)
                throw new DomainException("User id cannot be empty.");

            if (!_projetoAcl.ProjetoEstaAtivo(tarefa.ProjetoId))
                throw new DomainException("Cannot assign a task from an archived project.");

            if (!_projetoAcl.UsuarioPertenceAoProjeto(tarefa.ProjetoId, usuarioId))
                throw new DomainException("Only project members can be assigned to tasks.");

            tarefa.AtribuirResponsavel(usuarioId);
        }
    }
}
