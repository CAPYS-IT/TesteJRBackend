using apiToDo.DTO;
using apiToDo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web.Http;
using System.Collections.Generic;

namespace apiToDo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefasController : ControllerBase
    {
        private Tarefas _tarefas;
        public TarefasController(){
            _tarefas= new Tarefas();
        }
        [Authorize]
        [HttpPost("lstTarefas")]
        public ActionResult lstTarefas()
        {   
            var tarefas = _tarefas.lstTarefas();

        return Ok(tarefas);//Contém o método lsTarefas que retorna a lista de tarefas.

            try
            {
              
                return StatusCode(200);
            }

            catch (Exception ex)
            {
                return StatusCode(400, new { msg = $"Ocorreu um erro em sua API {ex.Message}"});
            }
        }

        [HttpPost("InserirTarefas")]
        public ActionResult InserirTarefas([FromBody] TarefaDTO Request)
        {   
            _tarefas.InserirTarefa(tarefa);
            var tarefas = _tarefas.lstTarefas();
            return Ok(tarefas); //Contém o método InserirTarefa que adiciona uma nova tarefa.

            try
            {

                return StatusCode(200);


            }

            catch (Exception ex)
            {
                return StatusCode(400, new { msg = $"Ocorreu um erro em sua API {ex.Message}" });
            }
        }

        [HttpGet("DeletarTarefa")]
        public ActionResult DeleteTask([FromQuery] int ID_TAREFA)
        {
            _tarefas.DeletarTarefa(ID_TAREFA);
            var tarefas = _tarefas.lstTarefas();
            return Ok(tarefas); //Contém o método DeletarTarefa que remove uma tarefa pelo seu ID.

            try
            {

                return StatusCode(200);
            }

            catch (Exception ex)
            {
                return StatusCode(400, new { msg = $"Ocorreu um erro em sua API {ex.Message}" });
            }
        }
    }
}
