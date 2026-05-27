using System;
using System.Collections.Generic;
using System.Linq;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Projects
{
    public class Projeto : Entity
    {
        private readonly List<MembroProjeto> _membros;

        public NomeProjeto Nome { get; private set; }
        public StatusProjeto Status { get; private set; }

        public IReadOnlyCollection<MembroProjeto> Membros
        {
            get { return _membros.AsReadOnly(); }
        }

        public Projeto(Guid id, NomeProjeto nome, MembroProjeto membroResponsavel)
            : base(id)
        {
            if (nome == null)
                throw new DomainException("Project name is required.");

            if (membroResponsavel == null)
                throw new DomainException("A project must have an initial responsible member.");

            if (!membroResponsavel.Responsavel)
                throw new DomainException("The initial project member must be responsible for the project.");

            Nome = nome;
            Status = StatusProjeto.Ativo;
            _membros = new List<MembroProjeto> { membroResponsavel };
        }

        public bool EstaAtivo()
        {
            return Status == StatusProjeto.Ativo;
        }

        public bool EstaArquivado()
        {
            return Status == StatusProjeto.Arquivado;
        }

        public void AlterarNome(NomeProjeto novoNome)
        {
            if (novoNome == null)
                throw new DomainException("Project name is required.");

            GarantirProjetoAtivo();

            Nome = novoNome;
        }

        public void AdicionarMembro(Guid usuarioId, string nome, bool responsavel)
        {
            GarantirProjetoAtivo();

            if (usuarioId == Guid.Empty)
                throw new DomainException("User id cannot be empty.");

            if (_membros.Any(m => m.UsuarioId == usuarioId))
                throw new DomainException("User is already a member of this project.");

            var membro = new MembroProjeto(
                Guid.NewGuid(),
                usuarioId,
                nome,
                responsavel);

            _membros.Add(membro);
        }

        public void RemoverMembro(Guid usuarioId)
        {
            GarantirProjetoAtivo();

            var membro = _membros.FirstOrDefault(m => m.UsuarioId == usuarioId);

            if (membro == null)
                throw new DomainException("User is not a member of this project.");

            if (membro.Responsavel && QuantidadeResponsaveis() == 1)
                throw new DomainException("A project must have at least one responsible member.");

            _membros.Remove(membro);
        }

        public bool UsuarioPertenceAoProjeto(Guid usuarioId)
        {
            return _membros.Any(m => m.UsuarioId == usuarioId);
        }

        public void Arquivar()
        {
            if (Status == StatusProjeto.Arquivado)
                throw new DomainException("Project is already archived.");

            if (QuantidadeResponsaveis() == 0)
                throw new DomainException("A project cannot be archived without a responsible member.");

            Status = StatusProjeto.Arquivado;
        }

        private int QuantidadeResponsaveis()
        {
            return _membros.Count(m => m.Responsavel);
        }

        private void GarantirProjetoAtivo()
        {
            if (Status == StatusProjeto.Arquivado)
                throw new DomainException("Archived projects cannot be changed.");
        }
    }
}
