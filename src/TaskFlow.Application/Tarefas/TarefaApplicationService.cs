using System;
using System.Collections.Generic;
using TaskFlow.Domain.Acl;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Application.Tarefas
{
    public class TarefaApplicationService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly ITarefaFactory _tarefaFactory;
        private readonly IProjetoAcl _projetoAcl;
        private readonly AlocacaoTarefaService _alocacaoTarefaService;
        private readonly ConclusaoTarefaService _conclusaoTarefaService;
        private readonly VerificadorPrazoService _verificadorPrazoService;

        public TarefaApplicationService(
            ITarefaRepository tarefaRepository,
            ITarefaFactory tarefaFactory,
            IProjetoAcl projetoAcl,
            AlocacaoTarefaService alocacaoTarefaService,
            ConclusaoTarefaService conclusaoTarefaService,
            VerificadorPrazoService verificadorPrazoService)
        {
            if (tarefaRepository == null)
                throw new ArgumentNullException(nameof(tarefaRepository));

            if (tarefaFactory == null)
                throw new ArgumentNullException(nameof(tarefaFactory));

            if (projetoAcl == null)
                throw new ArgumentNullException(nameof(projetoAcl));

            if (alocacaoTarefaService == null)
                throw new ArgumentNullException(nameof(alocacaoTarefaService));

            if (conclusaoTarefaService == null)
                throw new ArgumentNullException(nameof(conclusaoTarefaService));

            if (verificadorPrazoService == null)
                throw new ArgumentNullException(nameof(verificadorPrazoService));

            _tarefaRepository = tarefaRepository;
            _tarefaFactory = tarefaFactory;
            _projetoAcl = projetoAcl;
            _alocacaoTarefaService = alocacaoTarefaService;
            _conclusaoTarefaService = conclusaoTarefaService;
            _verificadorPrazoService = verificadorPrazoService;
        }

        public Tarefa CriarTarefa(
            Guid projetoId,
            string titulo,
            string descricao,
            Prioridade prioridade,
            DateTime dataLimite)
        {
            if (!_projetoAcl.ProjetoEstaAtivo(projetoId))
                throw new DomainException("Cannot create a task for an archived or non-existing project.");

            var tarefa = _tarefaFactory.Criar(
                projetoId,
                titulo,
                descricao,
                prioridade,
                dataLimite);

            _tarefaRepository.Adicionar(tarefa);

            return tarefa;
        }

        public void AtribuirResponsavel(Guid tarefaId, Guid usuarioId)
        {
            var tarefa = ObterTarefaObrigatoria(tarefaId);

            _alocacaoTarefaService.AtribuirResponsavel(tarefa, usuarioId);

            _tarefaRepository.Atualizar(tarefa);
        }

        public void IniciarTarefa(Guid tarefaId)
        {
            var tarefa = ObterTarefaObrigatoria(tarefaId);

            tarefa.Iniciar();

            _tarefaRepository.Atualizar(tarefa);
        }

        public void ConcluirTarefa(Guid tarefaId)
        {
            var tarefa = ObterTarefaObrigatoria(tarefaId);

            _conclusaoTarefaService.Concluir(tarefa);

            _tarefaRepository.Atualizar(tarefa);
        }

        public bool TarefaEstaAtrasada(Guid tarefaId, DateTime dataAtual)
        {
            var tarefa = ObterTarefaObrigatoria(tarefaId);

            return _verificadorPrazoService.EstaAtrasada(tarefa, dataAtual);
        }

        public IEnumerable<Tarefa> ObterTarefasPorProjeto(Guid projetoId)
        {
            if (projetoId == Guid.Empty)
                throw new DomainException("Project id cannot be empty.");

            return _tarefaRepository.ObterPorProjeto(projetoId);
        }

        private Tarefa ObterTarefaObrigatoria(Guid tarefaId)
        {
            if (tarefaId == Guid.Empty)
                throw new DomainException("Task id cannot be empty.");

            var tarefa = _tarefaRepository.ObterPorId(tarefaId);

            if (tarefa == null)
                throw new DomainException("Task was not found.");

            return tarefa;
        }
    }
}
