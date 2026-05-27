using System;

namespace TaskFlow.Domain.Acl
{
    public interface IProjetoAcl
    {
        bool ProjetoEstaAtivo(Guid projetoId);
        bool UsuarioPertenceAoProjeto(Guid projetoId, Guid usuarioId);
    }
}
