using System;

namespace TaskFlow.Domain.Projects
{
    public interface IProjetoRepository
    {
        Projeto ObterPorId(Guid id);
        void Adicionar(Projeto projeto);
        void Atualizar(Projeto projeto);
    }
}
