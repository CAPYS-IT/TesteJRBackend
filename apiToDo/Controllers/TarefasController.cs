using apiToDo.DTO;
using apiToDo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace apiToDo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private static List<TarefaDTO> _tarefas = new List<TarefaDTO>();
        private static int _idCounter = 1;

        /// <summary>
        /// Retorna lista completa de tarefas
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<TarefaDTO>> GetAll()
        {
            try
            {
                return Ok(_tarefas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Erro interno ao listar tarefas",
                    error = ex.Message
                });

            }
        }

        /// <summary>
        /// Retorna uma tarefa específica por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<TarefaDTO> GetById(int id)
        {
            try
            {
                var tarefa = _tarefas.FirstOrDefault(t => t.ID_TAREFA == id);

                if(tarefa == null)
                {
                    return NotFound(new {msg = $"Tarefa com ID {id} não encontrada!"});
                }

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = $"Erro ao buscar tarefa {id}",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Cria uma nova tarefa
        /// </summary>
        [HttpPost]
        public ActionResult <TarefaDTO> Create([FromBody] TarefaDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.DS_TAREFA))
                {
                    return BadRequest(new 
                    {
                        msg = "Descrição da tarefa é obrigatória!" 
                    });
                }
                request.ID_TAREFA = _idCounter++;
                _tarefas.Add(request);
              
                return CreatedAtAction(nameof(GetById), new {id = request.ID_TAREFA}, request);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    msg = $"Ocorreu um erro ao criar tarefa: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Atualiza uma tarefa existente
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<TarefaDTO> Update(int id, [FromBody] TarefaDTO request)
        {
            try
            {
                var tarefaExistente = _tarefas.FirstOrDefault(t => t.ID_TAREFA == id);

                if (tarefaExistente == null) 
                {
                    return NotFound(new { 
                        msg = $"Tarefa com ID {id} não encontrada!" 
                    });
                }
                tarefaExistente.DS_TAREFA = request.DS_TAREFA;

                return Ok(tarefaExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = $"Erro ao atualizar tarefa {id}",
                    error = ex.Message
                });
                throw;
            }
        }

        /// <summary>
        /// Remove uma tarefa
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteTask(int id)
        {
            try
            {
                var tarefa = _tarefas.FirstOrDefault(t => t.ID_TAREFA == id);

                if (tarefa == null)
                {
                    return NotFound(new { 
                        msg = $"Tarefa com ID {id} não encontrada!" 
                    });
                }

                _tarefas.Remove(tarefa);
                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    msg = $"Ocorreu um erro ao deletar a tarefa {id}",
                    error = ex.Message
                });
            }
        }
    }
}
