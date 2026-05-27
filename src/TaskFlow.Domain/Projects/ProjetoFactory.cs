using System;

namespace TaskFlow.Domain.Projects
{
    public class ProjetoFactory
    {
        public Projeto Criar(string nomeProjeto, Guid usuarioResponsavelId, string nomeResponsavel)
        {
            var nome = new NomeProjeto(nomeProjeto);

            var membroResponsavel = new MembroProjeto(
                Guid.NewGuid(),
                usuarioResponsavelId,
                nomeResponsavel,
                true);

            return new Projeto(
                Guid.NewGuid(),
                nome,
                membroResponsavel);
        }
    }
}
