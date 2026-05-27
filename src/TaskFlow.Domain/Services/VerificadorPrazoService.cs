using System;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Domain.Services
{
    public class VerificadorPrazoService
    {
        public bool EstaAtrasada(Tarefa tarefa, DateTime dataAtual)
        {
            if (tarefa == null)
                throw new DomainException("Task is required.");

            return !tarefa.EstaConcluida() && tarefa.Prazo.EstaVencido(dataAtual);
        }
    }
}
