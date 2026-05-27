using System;
using TaskFlow.Domain.Acl;
using TaskFlow.Domain.Projects;

namespace TaskFlow.Infrastructure.Acl
{
    public class ProjetoAclAdapter : IProjetoAcl
    {
        private readonly IProjetoRepository _projetoRepository;

        public ProjetoAclAdapter(IProjetoRepository projetoRepository)
        {
            if (projetoRepository == null)
                throw new ArgumentNullException(nameof(projetoRepository));

            _projetoRepository = projetoRepository;
        }

        public bool ProjetoEstaAtivo(Guid projetoId)
        {
            var projeto = _projetoRepository.ObterPorId(projetoId);

            if (projeto == null)
                return false;

            return projeto.EstaAtivo();
        }

        public bool UsuarioPertenceAoProjeto(Guid projetoId, Guid usuarioId)
        {
            var projeto = _projetoRepository.ObterPorId(projetoId);

            if (projeto == null)
                return false;

            return projeto.UsuarioPertenceAoProjeto(usuarioId);
        }
    }
}
