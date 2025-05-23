using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using servicio.Data;
using servicio.Models.ModelsDTO;
using servicio.Models;
using Microsoft.EntityFrameworkCore;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly MyAppContext _appContext;
        public HorarioController(MyAppContext appContext)
        {
            _appContext = appContext;
        }


        [HttpGet]
        public IActionResult GetAllHorarios()
        {
            var horarios = _appContext.Horarios
                .Include(p => p.GradoSeccion)
                .ToList();
            return Ok(horarios);
        }
        [HttpGet]
        [Route("porGradoSeccion/{id}")]
        public IActionResult GetHorariosPorGradoSeccion(int id)
        {
            try
            {
                var horarios = _appContext.Horarios
                    .Include(p => p.GradoSeccion)
                    .Where(h => h.GradoSeccionId == id)
                    .ToList();
                return Ok(horarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.ToString()}");
            }
        }
        [HttpPost]
        public IActionResult AddHorario([FromBody] HorarioDTO horarioDTO)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var horario = new Horario
                    {
                        HoraInicio = TimeSpan.Parse(horarioDTO.HoraInicio),
                        HoraFin = TimeSpan.Parse(horarioDTO.HoraFin),
                        DiaSemana = horarioDTO.DiaSemana,
                        GradoSeccionId = horarioDTO.GradoSeccionId
                    };

                    _appContext.Horarios.Add(horario);
                    _appContext.SaveChanges();


                    var horarioConGradoSeccion = _appContext.Horarios
                        .Include(p => p.GradoSeccion)
                        .FirstOrDefault(p => p.Id == horario.Id);

                    transaction.Commit();
                    return Ok(horarioConGradoSeccion);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditHorario(int id, [FromBody] HorarioDTO horarioDTO)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var horario = _appContext.Horarios.FirstOrDefault(c => c.Id == id);
                    if (horario == null)
                    {
                        return NotFound($"Horario con ID {id} no encontrado");
                    }

                    horario.HoraInicio = TimeSpan.Parse(horarioDTO.HoraInicio);
                    horario.HoraFin = TimeSpan.Parse(horarioDTO.HoraFin);
                    horario.DiaSemana = horarioDTO.DiaSemana;
                    horario.GradoSeccionId = horarioDTO.GradoSeccionId;

                    _appContext.SaveChanges();
                    transaction.Commit();
                    return Ok($"Horario con ID {id} ha sido actualizado");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHorario(int id)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var horario = _appContext.Horarios.FirstOrDefault(c => c.Id == id);
                    if (horario == null)
                    {
                        return NotFound($"Horario con ID {id} no encontrado");
                    }

                    _appContext.Horarios.Remove(horario);
                    _appContext.SaveChanges();
                    transaction.Commit();
                    return Ok($"Horario con ID {id} ha sido eliminado");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {e.Message}");
                }
            }
        }
    }
}
