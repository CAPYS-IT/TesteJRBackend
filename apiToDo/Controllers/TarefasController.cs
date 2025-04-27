using Microsoft.AspNetCore.Mvc;
using apiToDo.DTO;
using apiToDo.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly Tarefas _tarefas;
        private readonly ILogger<TarefasController> _logger;

        public TarefasController(Tarefas tarefas, ILogger<TarefasController> logger)
        {
            _tarefas = tarefas;
            _logger = logger;
        }

        // Retorna a lista de tarefas
        [HttpGet("lstTarefas")]
        public ActionResult<List<TarefaDTO>> lstTarefas()
        {
            try
            {
                var tarefasList = _tarefas.lstTarefas();
                _logger.LogInformation("Lista de tarefas retornada com sucesso.");
                return Ok(tarefasList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao listar tarefas: {ex.Message}");
                return StatusCode(500, new { Message = "Erro ao listar tarefas: " + ex.Message });
            }
        }

        // Retorna um item da lista com base no ID
        [HttpGet("{id}")]
        public ActionResult<TarefaDTO> GetTarefa(int id)
        {
            try
            {
                var tarefa = _tarefas.lstTarefas().FirstOrDefault(t => t.ID_TAREFA == id);

                if (tarefa == null)
                    return NotFound(new { Message = $"Tarefa com ID {id} não encontrada." });

                _logger.LogInformation($"Tarefa com ID {id} retornada com sucesso.");
                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter tarefa: {ex.Message}");
                return StatusCode(500, new { Message = "Erro ao obter tarefa: " + ex.Message });
            }
        }

      
        // Insere uma nova tarefa
        [HttpPost("Inserir")]
        public async Task<ActionResult<List<TarefaDTO>>> InserirTarefa([FromBody] TarefaDTO novaTarefa)
        {
            try
            {
                if (novaTarefa == null)
                    return BadRequest(new { Message = "A tarefa não pode ser nula." });

                await _tarefas.InserirOuAtualizarTarefaAsync(novaTarefa); // Inserção nova

                var tarefasList = _tarefas.lstTarefas(); // Retorna a lista de tarefas atualizada

                _logger.LogInformation("Nova tarefa inserida com sucesso.");
                return Ok(tarefasList);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Erro de validação: {ex.Message}");
                return BadRequest(new { Message = $"Erro de validação: {ex.Message}" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao inserir tarefa: {ex.Message}");
                return StatusCode(500, new { Message = "Erro ao inserir tarefa: " + ex.Message });
            }
        }



        // Deleta uma tarefa com base no ID
        [HttpDelete("DeleteTask/{id}")]
        public async Task<ActionResult<List<TarefaDTO>>> DeletarTarefa(int id)
        {
            try
            {
                // Chama o método DeletarTarefaAsync da camada de serviço (_tarefas) para deletar a tarefa com o ID fornecido
                await _tarefas.DeletarTarefaAsync(id);

                // Registra uma mensagem de sucesso no log, indicando que a tarefa foi deletada
                _logger.LogInformation($"Tarefa com ID {id} deletada com sucesso.");

                // Chama o método lstTarefas() para obter a lista de tarefas atualizada após a exclusão
                var tarefasList = _tarefas.lstTarefas();

                // Retorna a lista de tarefas atualizada com o status HTTP 200 (OK)
                return Ok(tarefasList);
            }
            catch (KeyNotFoundException ex)
            {
                // Se a tarefa não for encontrada (lançando KeyNotFoundException), registra um aviso no log
                _logger.LogWarning($"Tarefa não encontrada: {ex.Message}");

                // Retorna o status HTTP 404 (NotFound) com uma mensagem indicando que a tarefa não foi encontrada
                return NotFound(new { Message = $"Tarefa com ID {id} não encontrada." });
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, registra o erro no log
                _logger.LogError($"Erro ao deletar tarefa: {ex.Message}");

                // Retorna o status HTTP 500 (Internal Server Error) com uma mensagem detalhada sobre o erro
                return StatusCode(500, new { Message = "Erro ao deletar tarefa: " + ex.Message });
            }
        }
        // Atualiza uma tarefa existente
        [HttpPut("Atualizar/{id}")]
        public async Task<ActionResult<List<TarefaDTO>>> AtualizarTarefa(int id, [FromBody] TarefaDTO tarefaAtualizada)
        {
            try
            {
                // Verifica se a tarefa recebida é nula ou a descrição está vazia
                if (tarefaAtualizada == null || string.IsNullOrWhiteSpace(tarefaAtualizada.DS_TAREFA))
                    return BadRequest(new { Message = "A tarefa ou a descrição não pode ser nula ou vazia." });

                // Tenta atualizar a tarefa diretamente, sem buscar novamente na lista
                tarefaAtualizada.ID_TAREFA = id; // Define o ID a ser atualizado
                await _tarefas.InserirOuAtualizarTarefaAsync(tarefaAtualizada); // Método de inserção/atualização no serviço

                // Retorna a lista atualizada de tarefas
                var tarefasList = _tarefas.lstTarefas();

                _logger.LogInformation($"Tarefa com ID {id} atualizada com sucesso.");
                return Ok(tarefasList);
            }
            catch (ArgumentException ex)
            {
                // Se houver erro de validação
                _logger.LogWarning($"Erro de validação: {ex.Message}");
                return BadRequest(new { Message = $"Erro de validação: {ex.Message}" });
            }
            catch (Exception ex)
            {
                // Para erros inesperados
                _logger.LogError($"Erro ao atualizar tarefa: {ex.Message}");
                return StatusCode(500, new { Message = "Erro ao atualizar tarefa: " + ex.Message });
            }
        }



    }
}
