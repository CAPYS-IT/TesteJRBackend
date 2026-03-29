using apiToDo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace apiToDo.Models
{
    public class TarefaService
    {
        private static List<TarefaDTO> _tarefas = new List<TarefaDTO>
        {
            new TarefaDTO { ID_TAREFA = 1, DS_TAREFA = "Fazer Compras" },
            new TarefaDTO { ID_TAREFA = 2, DS_TAREFA = "Fazer Atividade Faculdade" },
            new TarefaDTO { ID_TAREFA = 3, DS_TAREFA = "Subir Projeto de Teste no GitHub" }
        };
        private static int _nextId = 4;

        /// <summary>
        /// Retorna todas as tarefas cadastradas
        /// </summary>
        public List<TarefaDTO> ObterTodasTarefas()
        {
            return _tarefas;
        }

        /// <summary>
        /// Adiciona uma nova tarefa
        /// </summary>
        public TarefaDTO AdicionarTarefa(TarefaDTO novaTarefa)
        {
            if (string.IsNullOrEmpty(novaTarefa.DS_TAREFA))
                throw new ArgumentException("Descrição da tarefa é obrigatória");

            novaTarefa.ID_TAREFA = _nextId++;
            _tarefas.Add(novaTarefa);
            return novaTarefa;
        }

        /// <summary>
        /// Remove uma tarefa existente
        /// </summary>
        public void RemoverTarefa(int id)
        {
            var tarefa = _tarefas.FirstOrDefault(t => t.ID_TAREFA == id);

            if (tarefa == null)
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada");

            _tarefas.Remove(tarefa);
        }

        /// <summary>
        /// Atualiza uma tarefa existente
        /// </summary>
        public TarefaDTO AtualizarTarefa(TarefaDTO tarefaAtualizada)
        {
            var tarefaExistente = _tarefas.FirstOrDefault(t => t.ID_TAREFA == tarefaAtualizada.ID_TAREFA);

            if (tarefaExistente == null)
                throw new KeyNotFoundException($"Tarefa com ID {tarefaAtualizada.ID_TAREFA} não encontrada");

            tarefaExistente.DS_TAREFA = tarefaAtualizada.DS_TAREFA;
            return tarefaExistente;
        }
    }
}
