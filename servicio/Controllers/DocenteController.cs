using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using servicio.Data;
using servicio.Models;
using servicio.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocenteController : ControllerBase
    {
        private readonly MyAppContext _appContext;

        public DocenteController(MyAppContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllDocentes()
        {
            var docentes = _appContext.Docentes.ToList();
            return Ok(docentes);
        }

        [HttpPost]
        public IActionResult AddDocente([FromBody] DocenteDTO docenteDTO)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var docente = new Docente
                    {
                        Nombre = docenteDTO.Nombre,
                        Apellido = docenteDTO.Apellido,
                        Dni = docenteDTO.Dni,
                        Especialidad = docenteDTO.Especialidad,
                        Estado = docenteDTO.Estado
                    };

                    _appContext.Docentes.Add(docente);
                    _appContext.SaveChanges();

                    transaction.Commit();
                    return Ok(docente);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditDocente(int id, [FromBody] DocenteDTO docenteDTO)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var docente = _appContext.Docentes.FirstOrDefault(d => d.Id == id);
                    if (docente == null)
                    {
                        return NotFound($"Docente con ID {id} no encontrado");
                    }

                    docente.Nombre = docenteDTO.Nombre;
                    docente.Apellido = docenteDTO.Apellido;
                    docente.Dni = docenteDTO.Dni;
                    docente.Especialidad = docenteDTO.Especialidad;
                    docente.Estado = docenteDTO.Estado;

                    _appContext.SaveChanges();
                    transaction.Commit();

                    return Ok($"Docente con ID {id} ha sido actualizado.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }
        [HttpDelete("(id)")]
        public IActionResult DeleteDocente(int id)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var docente = _appContext.Docentes.FirstOrDefault(b => b.Id == id);

                    if (docente == null)
                    {
                        return NotFound($"Docente con ID {id} no encontrado");
                    }

                    _appContext.Docentes.Remove(docente);
                    _appContext.SaveChanges();

                    transaction.Commit();

                    return Ok($"Docente con ID {id} ha sido eliminado");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpPut("desactivate/{id}")]
        public IActionResult DesactivateDocente(int id)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var docente = _appContext.Docentes.FirstOrDefault(d => d.Id == id);
                    if (docente == null)
                    {
                        return NotFound($"Docente con ID {id} no encontrado.");
                    }

                    docente.Estado = false;
                    _appContext.SaveChanges();
                    transaction.Commit();

                    return Ok($"Docente con ID {id} ha sido desactivado");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpPut("reinstate/{id}")]
        public IActionResult ReinstateDocente(int id)
        {
            using (var transaction = _appContext.Database.BeginTransaction())
            {
                try
                {
                    var docente = _appContext.Docentes.FirstOrDefault(d => d.Id == id);
                    if (docente == null)
                    {
                        return NotFound($"Docente con ID {id} no encontrado.");
                    }

                    docente.Estado = true;
                    _appContext.SaveChanges();
                    transaction.Commit();

                    return Ok($"Docente con ID {id} ha sido reactivado.");
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
