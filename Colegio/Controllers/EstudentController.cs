using Colegio.Models;
using Colegio.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudentController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<EstudentController> _logger;
        private ColegioContext _dbcontext;
        private readonly IEstudent _estudent;

        public EstudentController(ILogger<EstudentController> logger, ColegioContext dbcontext, IEstudent estudent)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _estudent = estudent;
        }

        [HttpGet]
        [Route("CreateDatabase")]
        public IActionResult CreateDatabase()
        {
            try
            {
                _logger.LogInformation("Creando base de datos");
                return Ok(_dbcontext.Database.EnsureCreated());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creando base de datos {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEstudent")]
        public IActionResult GetEstudent()
        {

            return Ok(_estudent.GetEstudents());
        }

        [HttpGet]
        [Route("GetEstudentByIdentification/{identification}")]
        public IActionResult GetEstudentByIdentification(int identification)
        {
            return Ok(_estudent.GetEstudentByIdeentification(identification));
        }

        [HttpPost]
        [Route("CreateEstudent")]
        public IActionResult CreateUser(EstudentDto estudent)
        {
            return Ok(_estudent.CreateEstudent(estudent));
        }

        [HttpDelete]
        [Route("DeleteEstudent/{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            try
            {
                if (_estudent.DeleteEstudent(id))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("No se pudo eliminar correctamente el usuario");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error eliminando usuarios {ex.Message.ToString()}");
            }
        }


    }
}