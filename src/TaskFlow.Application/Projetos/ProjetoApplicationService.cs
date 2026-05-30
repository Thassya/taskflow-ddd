using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Projects;

namespace TaskFlow.Application.Projetos
{
    public class ProjetoApplicationService
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IProjetoFactory _projetoFactory;

        public ProjetoApplicationService(
            IProjetoRepository projetoRepository,
            IProjetoFactory projetoFactory)
        {
            if (projetoRepository == null)
                throw new ArgumentNullException(nameof(projetoRepository));

            if (projetoFactory == null)
                throw new ArgumentNullException(nameof(projetoFactory));

            _projetoRepository = projetoRepository;
            _projetoFactory = projetoFactory;
        }

        public Projeto CriarProjeto(
            string nomeProjeto,
            Guid usuarioResponsavelId,
            string nomeResponsavel)
        {
            var projeto = _projetoFactory.Criar(
                nomeProjeto,
                usuarioResponsavelId,
                nomeResponsavel);

            _projetoRepository.Adicionar(projeto);

            return projeto;
        }

        public void AdicionarMembro(
            Guid projetoId,
            Guid usuarioId,
            string nome,
            bool responsavel)
        {
            var projeto = ObterProjetoObrigatorio(projetoId);

            projeto.AdicionarMembro(
                usuarioId,
                nome,
                responsavel);

            _projetoRepository.Atualizar(projeto);
        }

        public void ArquivarProjeto(Guid projetoId)
        {
            var projeto = ObterProjetoObrigatorio(projetoId);

            projeto.Arquivar();

            _projetoRepository.Atualizar(projeto);
        }

        private Projeto ObterProjetoObrigatorio(Guid projetoId)
        {
            if (projetoId == Guid.Empty)
                throw new DomainException("Project id cannot be empty.");

            var projeto = _projetoRepository.ObterPorId(projetoId);

            if (projeto == null)
                throw new DomainException("Project was not found.");

            return projeto;
        }
    }
}
