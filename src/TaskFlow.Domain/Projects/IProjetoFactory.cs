using System;

namespace TaskFlow.Domain.Projects
{
    public interface IProjetoFactory
    {
        Projeto Criar(string nomeProjeto, Guid usuarioResponsavelId, string nomeResponsavel);
    }
}
