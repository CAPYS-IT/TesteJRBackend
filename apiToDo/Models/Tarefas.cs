using apiToDo.DTO;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace apiToDo.Models
{
    public class Tarefas
    {
        private readonly ILogger<Tarefas> _logger;

        // Lista em memória simulando um "banco de dados"
        private static List<TarefaDTO> _tarefas = new List<TarefaDTO>();
        private static int _proximoId = 1;

        public Tarefas(ILogger<Tarefas> logger)
        {
            _logger = logger;
        }

        // Método que retorna a lista de tarefas
        public List<TarefaDTO> lstTarefas()
        {
            return _tarefas.ToList(); // Retorna todas as tarefas
        }

        // Método que insere ou atualiza uma tarefa individual
        public Task InserirOuAtualizarTarefaAsync(TarefaDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "A tarefa não pode ser nula.");

            if (string.IsNullOrWhiteSpace(request.DS_TAREFA))
                throw new ArgumentException("A descrição da tarefa não pode estar vazia.");

            if (request.ID_TAREFA > 0)
            {
                // Atualização por ID
                var tarefaExistente = _tarefas.FirstOrDefault(t => t.ID_TAREFA == request.ID_TAREFA);

                if (tarefaExistente == null)
                    throw new KeyNotFoundException($"Tarefa com ID {request.ID_TAREFA} não encontrada para atualização.");

                tarefaExistente.DS_TAREFA = request.DS_TAREFA;
                _logger.LogInformation($"Tarefa com ID {request.ID_TAREFA} atualizada com sucesso.");
            }
            else
            {
                // Inserção
                var descricaoNormalizada = request.DS_TAREFA.Trim().ToLower();

                var tarefaDuplicada = _tarefas.FirstOrDefault(t => t.DS_TAREFA.Trim().ToLower() == descricaoNormalizada);

                if (tarefaDuplicada != null)
                    throw new ArgumentException($"Tarefa com descrição '{request.DS_TAREFA}' já existe.");

                request.ID_TAREFA = _proximoId++;
                _tarefas.Add(request);
                _logger.LogInformation($"Tarefa '{request.DS_TAREFA}' inserida com sucesso.");
            }

            return Task.CompletedTask;
        }

        // NOVO MÉTODO: Inserir uma lista de tarefas
        public Task InserirTarefasAsync(List<TarefaDTO> tarefas)
        {
            if (tarefas == null || tarefas.Count == 0)
                throw new ArgumentException("A lista de tarefas não pode ser nula ou vazia.");

            foreach (var tarefa in tarefas)
            {
                if (string.IsNullOrWhiteSpace(tarefa.DS_TAREFA))
                    throw new ArgumentException("A descrição da tarefa não pode estar vazia.");

                var descricaoNormalizada = tarefa.DS_TAREFA.Trim().ToLower();

                // Verificando duplicidade antes da inserção
                var tarefaExistente = _tarefas.FirstOrDefault(t => t.DS_TAREFA.Trim().ToLower() == descricaoNormalizada);

                if (tarefaExistente != null)
                {
                    // Garantir que a mensagem de erro é exibida
                    _logger.LogInformation($"Tarefa com descrição '{tarefa.DS_TAREFA}' já existe.");
                    throw new ArgumentException($"Tarefa com descrição '{tarefa.DS_TAREFA}' já existe.");
                }

                tarefa.ID_TAREFA = _proximoId++;
                _tarefas.Add(tarefa);
                _logger.LogInformation($"Tarefa '{tarefa.DS_TAREFA}' inserida com sucesso.");
            }

            return Task.CompletedTask;
        }

        // Método que deleta uma tarefa
        public Task DeletarTarefaAsync(int ID_TAREFA)
        {
            var tarefa = _tarefas.FirstOrDefault(t => t.ID_TAREFA == ID_TAREFA);
            if (tarefa == null)
                throw new KeyNotFoundException($"Tarefa com ID {ID_TAREFA} não encontrada.");

            _tarefas.Remove(tarefa);
            _logger.LogInformation($"Tarefa com ID {ID_TAREFA} removida com sucesso.");

            return Task.CompletedTask;
        }
    }
}
