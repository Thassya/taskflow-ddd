using System;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Projects
{
    public class MembroProjeto : Entity
    {
        public Guid UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public bool Responsavel { get; private set; }

        public MembroProjeto(Guid id, Guid usuarioId, string nome, bool responsavel)
            : base(id)
        {
            if (usuarioId == Guid.Empty)
                throw new DomainException("User id cannot be empty.");

            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Project member name cannot be empty.");

            UsuarioId = usuarioId;
            Nome = nome.Trim();
            Responsavel = responsavel;
        }

        public void TornarResponsavel()
        {
            Responsavel = true;
        }

        public void RemoverResponsabilidade()
        {
            Responsavel = false;
        }
    }
}
