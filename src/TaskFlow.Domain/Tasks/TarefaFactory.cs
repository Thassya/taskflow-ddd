using System;

namespace TaskFlow.Domain.Tasks
{
    public class TarefaFactory : ITarefaFactory
    {
        public Tarefa Criar(
            Guid projetoId,
            string titulo,
            string descricao,
            Prioridade prioridade,
            DateTime dataLimite)
        {
            return new Tarefa(
                Guid.NewGuid(),
                projetoId,
                new TituloTarefa(titulo),
                new DescricaoTarefa(descricao),
                prioridade,
                new Prazo(dataLimite));
        }
    }
}
