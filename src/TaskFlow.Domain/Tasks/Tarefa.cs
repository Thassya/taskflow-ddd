using System;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Tasks
{
    public class Tarefa : Entity
    {
        public Guid ProjetoId { get; private set; }
        public TituloTarefa Titulo { get; private set; }
        public DescricaoTarefa Descricao { get; private set; }
        public Prazo Prazo { get; private set; }
        public Prioridade Prioridade { get; private set; }
        public StatusTarefa Status { get; private set; }
        public Guid? ResponsavelId { get; private set; }

        public Tarefa(
            Guid id,
            Guid projetoId,
            TituloTarefa titulo,
            DescricaoTarefa descricao,
            Prioridade prioridade,
            Prazo prazo)
            : base(id)
        {
            if (projetoId == Guid.Empty)
                throw new DomainException("Project id cannot be empty.");

            if (titulo == null)
                throw new DomainException("Task title is required.");

            if (descricao == null)
                throw new DomainException("Task description is required.");

            if (prazo == null)
                throw new DomainException("Task due date is required.");

            ProjetoId = projetoId;
            Titulo = titulo;
            Descricao = descricao;
            Prioridade = prioridade;
            Prazo = prazo;
            Status = StatusTarefa.Backlog;
            ResponsavelId = null;
        }

        public void AlterarTitulo(TituloTarefa novoTitulo)
        {
            GarantirNaoConcluida();

            if (novoTitulo == null)
                throw new DomainException("Task title is required.");

            Titulo = novoTitulo;
        }

        public void AlterarDescricao(DescricaoTarefa novaDescricao)
        {
            GarantirNaoConcluida();

            if (novaDescricao == null)
                throw new DomainException("Task description is required.");

            Descricao = novaDescricao;
        }

        public void AlterarPrioridade(Prioridade novaPrioridade)
        {
            GarantirNaoConcluida();

            Prioridade = novaPrioridade;
        }

        public void AlterarPrazo(Prazo novoPrazo)
        {
            GarantirNaoConcluida();

            if (novoPrazo == null)
                throw new DomainException("Task due date is required.");

            Prazo = novoPrazo;
        }

        public void AtribuirResponsavel(Guid usuarioId)
        {
            GarantirNaoConcluida();

            if (usuarioId == Guid.Empty)
                throw new DomainException("Responsible user id cannot be empty.");

            ResponsavelId = usuarioId;
        }

        public void Iniciar()
        {
            GarantirNaoConcluida();

            if (Status != StatusTarefa.Backlog)
                throw new DomainException("Only backlog tasks can be started.");

            Status = StatusTarefa.EmAndamento;
        }

        public void Concluir()
        {
            GarantirNaoConcluida();

            if (!ResponsavelId.HasValue)
                throw new DomainException("A task cannot be completed without an assignee.");

            if (Status != StatusTarefa.EmAndamento)
                throw new DomainException("Only tasks in progress can be completed.");

            Status = StatusTarefa.Concluida;
        }

        public bool EstaConcluida()
        {
            return Status == StatusTarefa.Concluida;
        }

        private void GarantirNaoConcluida()
        {
            if (Status == StatusTarefa.Concluida)
                throw new DomainException("Completed tasks cannot be changed.");
        }
    }
}
