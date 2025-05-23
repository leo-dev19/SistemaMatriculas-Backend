using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using servicio.Data;
using servicio.Models;
using servicio.Models.ModelsDTO;
using System;
using System.Linq;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly MyAppContext _appContext;

        public MatriculaController(MyAppContext appContext)
        {
            _appContext = appContext;
        }

        // Obtener todas las matrículas con sus relaciones
        [HttpGet]
        public IActionResult GetAllMatriculas()
        {
            var matriculas = _appContext.Matriculas
                .Include(m => m.Student)
                .Include(m => m.Docente)
                .Include(m => m.Horario)
                .Include(m => m.GradoSeccion)
                .Include(m => m.LegalGuardian)
                .ToList();

            return Ok(matriculas);
        }

        // Obtener matrícula por ID
        [HttpGet("{id}")]
        public IActionResult GetMatriculaById(int id)
        {
            var matricula = _appContext.Matriculas
                .Include(m => m.Student)
                .Include(m => m.Docente)
                .Include(m => m.Horario)
                .Include(m => m.GradoSeccion)
                .Include(m => m.LegalGuardian)
                .FirstOrDefault(m => m.Id == id);

            if (matricula == null)
                return NotFound($"Matrícula con ID {id} no encontrada");

            return Ok(matricula);
        }

        // Crear una nueva matrícula
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddMatricula([FromBody] MatriculaDTO matriculaDTO)
        {
            using (var transaction = await _appContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var matricula = new Matricula
                    {
                        StudentId = matriculaDTO.StudentId,
                        DocenteId = matriculaDTO.DocenteId,
                        HorarioId = matriculaDTO.HorarioId,
                        GradoSeccionId = matriculaDTO.GradoSeccionId,
                        LegalGuardianId = matriculaDTO.LegalGuardianId,
                        FechaMatricula = matriculaDTO.FechaMatricula,
                        Estado = matriculaDTO.Estado
                    };

                    _appContext.Matriculas.Add(matricula);
                    await _appContext.SaveChangesAsync();

                    var savedMatricula = await _appContext.Matriculas
                        .Include(m => m.Student)
                        .Include(m => m.Docente)
                        .Include(m => m.Horario)
                        .Include(m => m.GradoSeccion)
                        .Include(m => m.LegalGuardian)
                        .FirstOrDefaultAsync(m => m.Id == matricula.Id);

                    await transaction.CommitAsync();
                    return Ok(savedMatricula);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error al agregar matrícula: {ex.Message}");
                }
            }
        }


        // Editar la matrícula
        [HttpPut("{id}")]
        public IActionResult EditMatricula(int id, [FromBody] MatriculaDTO matriculaDTO)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var matricula = _appContext.Matriculas.FirstOrDefault(m => m.Id == id);
                    if (matricula == null)
                        return NotFound($"Matrícula con ID {id} no encontrada");

                    matricula.StudentId = matriculaDTO.StudentId;
                    matricula.DocenteId = matriculaDTO.DocenteId;
                    matricula.HorarioId = matriculaDTO.HorarioId;
                    matricula.GradoSeccionId = matriculaDTO.GradoSeccionId;
                    matricula.LegalGuardianId = matriculaDTO.LegalGuardianId;
                    matricula.FechaMatricula = matriculaDTO.FechaMatricula;
                    matricula.Estado = matriculaDTO.Estado;

                    _appContext.SaveChanges();
                    transaction.Commit();

                    // Retornar el objeto actualizado, incluyendo las relaciones si es necesario:
                    var updatedMatricula = _appContext.Matriculas
                        .Include(m => m.Student)
                        .Include(m => m.Docente)
                        .Include(m => m.Horario)
                        .Include(m => m.GradoSeccion)
                        .Include(m => m.LegalGuardian)
                        .FirstOrDefault(m => m.Id == id);

                    return Ok(updatedMatricula);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Error al actualizar matrícula: {ex.Message}");
                }
            }
        }



        // Eliminar una matrícula
        [HttpDelete("{id}")]
        public IActionResult DeleteMatricula(int id)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var matricula = _appContext.Matriculas.FirstOrDefault(m => m.Id == id);
                    if (matricula == null)
                        return NotFound($"Matrícula con ID {id} no encontrada");

                    _appContext.Matriculas.Remove(matricula);
                    _appContext.SaveChanges();
                    transaction.Commit();
                    return Ok($"Matrícula con ID {id} eliminada correctamente");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Error al eliminar matrícula: {ex.Message}");
                }
            }
        }

        [HttpPut("desactivate/{id}")]
        public IActionResult DesactivateMatricula(int id)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var matricula = _appContext.Matriculas.FirstOrDefault(m => m.Id == id);
                    if (matricula == null)
                    {
                        return NotFound($"Matrícula con ID {id} no encontrada.");
                    }

                    matricula.Estado = false;
                    _appContext.SaveChanges();
                    transaction.Commit();

                    return Ok($"Matrícula con ID {id} ha sido desactivada.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpPut("reinstate/{id}")]
        public IActionResult ReinstateMatricula(int id)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var matricula = _appContext.Matriculas.FirstOrDefault(m => m.Id == id);
                    if (matricula == null)
                    {
                        return NotFound($"Matrícula con ID {id} no encontrada.");
                    }

                    matricula.Estado = true;
                    _appContext.SaveChanges();
                    transaction.Commit();

                    return Ok($"Matrícula con ID {id} ha sido reactivada.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }


    }
}
