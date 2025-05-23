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
    public class AsignacionDocenteController : ControllerBase
    {
        private readonly MyAppContext _appContext;

        public AsignacionDocenteController(MyAppContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        public IActionResult GetAllAsignaciones()
        {
            var asignacion = _appContext.AsignacionesDocentes
                .Include(p => p.GradoSeccion)
                .Include(p => p.Docente)
                .ToList();
            return Ok(asignacion);
        }

        // Obtener una sección por ID
        [HttpGet("{id}")]
        public IActionResult GetAsignacion(int id)
        {
            var asignaciones = _appContext.AsignacionesDocentes.FirstOrDefault(g => g.Id == id);
            if (asignaciones == null)
            {
                return NotFound($"Asignacion con ID {id} no encontrada.");
            }
            return Ok(asignaciones);
        }

        // Agregar una nueva sección
        [HttpPost]
        public IActionResult AddAsignaciones([FromBody] AsignacionDocenteDTO asignacionDocenteDTO)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var asignaciones = new AsignacionDocente
                    {
                        GradoSeccionId = asignacionDocenteDTO.GradoSeccionId,
                        DocenteId = asignacionDocenteDTO.DocenteId
                    };

                    _appContext.AsignacionesDocentes.Add(asignaciones);
                    _appContext.SaveChanges();


                    var asignacionConDocenteGradoSeccion = _appContext.AsignacionesDocentes
                        .Include(p => p.Docente)
                        .Include(p => p.GradoSeccion)
                        .FirstOrDefault(p => p.Id == asignaciones.Id);


                    transaction.Commit();
                    return Ok(asignacionConDocenteGradoSeccion);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        // Editar una sección existente
        [HttpPut("{id}")]
        public IActionResult EditAsignaciones(int id, [FromBody] AsignacionDocenteDTO asignacionDocenteDTO)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var asignaciones = _appContext.AsignacionesDocentes.FirstOrDefault(c => c.Id == id);
                    if (asignaciones == null)
                    {
                        return NotFound($"Asignacion con ID {id} no encontrado");
                    }

                    asignaciones.GradoSeccionId = asignacionDocenteDTO.GradoSeccionId;
                    asignaciones.DocenteId = asignacionDocenteDTO.DocenteId;

                    _appContext.SaveChanges();
                    transaction.Commit();
                    return Ok($"Asignacion con ID {id} ha sido actualizado");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        // Eliminar una sección
        [HttpDelete("{id}")]
        public IActionResult DeleteGradoSeccion(int id)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var asignaciones = _appContext.AsignacionesDocentes.FirstOrDefault(c => c.Id == id);
                    if (asignaciones == null)
                    {
                        return NotFound($"Asignacion con ID {id} no encontrado");
                    }

                    _appContext.AsignacionesDocentes.Remove(asignaciones);
                    _appContext.SaveChanges();
                    transaction.Commit();
                    return Ok($"Asignacion con ID {id} ha sido eliminado");
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
