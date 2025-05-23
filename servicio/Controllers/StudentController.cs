using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using servicio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProyectoDSWI.Models;
using servicio.Models.ModelsDTO;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly MyAppContext myAppContext;
        private readonly IWebHostEnvironment _env;

        public StudentController (MyAppContext myAppContext, IWebHostEnvironment env)
        {
            this.myAppContext = myAppContext;
            _env = env;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllStudents()
        {
            var allStudents = await myAppContext.Students
                                                .Include(s => s.LegalGuardian)
                                                .ToListAsync();

            if (allStudents == null || !allStudents.Any())
            {
                return NotFound("No se encontraron estudiantes.");
            }

            return Ok(allStudents);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterStudent([FromForm] StudentDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = await myAppContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Manejo del apoderado (LegalGuardian)...
                    LegalGuardian legalGuardian = null;
                    if (request.LegalGuardian != null)
                    {
                        legalGuardian = await myAppContext.LegalGuardians
                            .FirstOrDefaultAsync(g => g.IdentityDocument == request.LegalGuardian.IdentityDocument);
                        if (legalGuardian == null)
                        {
                            legalGuardian = new LegalGuardian
                            {
                                IdentityDocument = request.LegalGuardian.IdentityDocument,
                                Name = request.LegalGuardian.Name,
                                LastName = request.LegalGuardian.LastName,
                                Gender = request.LegalGuardian.Gender,
                                Birthdate = request.LegalGuardian.Birthdate,
                                CellphoneNumber = request.LegalGuardian.CellphoneNumber,
                                Email = request.LegalGuardian.Email,
                                Direction = request.LegalGuardian.Direction
                            };
                            myAppContext.LegalGuardians.Add(legalGuardian);
                            await myAppContext.SaveChangesAsync();
                        }
                    }

                    // Procesar la imagen
                    string imagenPath = null;
                    if (request.Imagen != null && request.Imagen.Length > 0)
                    {
                        // Usa el web root, inyectado a través de IWebHostEnvironment
                        string webRootPath = _env.WebRootPath ?? Directory.GetCurrentDirectory();
                        var imagesFolder = Path.Combine(webRootPath, "imagenes");

                        // Si la carpeta "imagenes" no existe, créala
                        if (!Directory.Exists(imagesFolder))
                        {
                            Directory.CreateDirectory(imagesFolder);
                        }

                        // Genera un nombre único para la imagen
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(request.Imagen.FileName)}";
                        var filePath = Path.Combine(imagesFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.Imagen.CopyToAsync(stream);
                        }
                        imagenPath = fileName; // Guarda el nombre o la ruta relativa en la BD
                    }

                    // Crear el registro del estudiante
                    var student = new Student
                    {
                        Code = request.Code,
                        Name = request.Name,
                        LastName = request.LastName,
                        Gender = request.Gender,
                        Direction = request.Direction,
                        Birthdate = request.Birthdate,
                        LegalGuardianId = legalGuardian?.Id,
                        ImagenPath = imagenPath
                    };

                    myAppContext.Students.Add(student);
                    await myAppContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(RegisterStudent), new { id = student.Id }, student);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Error al registrar el estudiante o apoderado: " + ex.Message);
                }
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditStudent(int id, [FromBody] StudentDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna los errores de validación si hay campos inválidos
            }

            // Buscar al estudiante en la base de datos
            var student = await myAppContext.Students.Include(s => s.LegalGuardian).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return NotFound("Estudiante no encontrado.");
            }

            // Verificar si el código del estudiante ya existe (en caso de modificación)
            var existingStudent = await myAppContext.Students
                                                 .Where(s => s.Code == request.Code && s.Id != id) // Asegurarse de que no se está editando el mismo estudiante
                                                 .FirstOrDefaultAsync();
            if (existingStudent != null)
            {
                return BadRequest("El código del estudiante ya existe.");
            }

            // Buscar al apoderado en la base de datos
            var legalGuardian = await myAppContext.LegalGuardians
                                              .FirstOrDefaultAsync(g => g.Id == student.LegalGuardianId);
            if (legalGuardian == null)
            {
                return NotFound("Apoderado no encontrado.");
            }

            // Verificar si el DNI del apoderado está siendo modificado y ya existe en la base de datos
            var existingLegalGuardian = await myAppContext.LegalGuardians
                                                      .Where(g => g.IdentityDocument == request.LegalGuardian.IdentityDocument && g.Id != legalGuardian.Id)
                                                      .FirstOrDefaultAsync();
            if (existingLegalGuardian != null)
            {
                return BadRequest("El DNI del apoderado ya está registrado.");
            }

            // Actualizar el estudiante
            student.Code = request.Code;
            student.Name = request.Name;
            student.LastName = request.LastName;
            student.Gender = request.Gender;
            student.Direction = request.Direction;
            student.Birthdate = request.Birthdate;

            // Si se ha modificado el apoderado, actualizamos sus datos
            legalGuardian.IdentityDocument = request.LegalGuardian.IdentityDocument;
            legalGuardian.Name = request.LegalGuardian.Name;
            legalGuardian.LastName = request.LegalGuardian.LastName;
            legalGuardian.Gender = request.LegalGuardian.Gender;
            legalGuardian.Birthdate = request.LegalGuardian.Birthdate;
            legalGuardian.CellphoneNumber = request.LegalGuardian.CellphoneNumber;
            legalGuardian.Email = request.LegalGuardian.Email;
            legalGuardian.Direction = request.LegalGuardian.Direction;

            // Guardar los cambios
            await myAppContext.SaveChangesAsync();

            return Ok(student); // Retorna el estudiante actualizado
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await myAppContext.Students.Include(s => s.LegalGuardian).FirstOrDefaultAsync(s => s.Id == id);
                if (student == null)
                {
                    return NotFound("Estudiante no encontrado.");
                }

                myAppContext.Students.Remove(student);
                await myAppContext.SaveChangesAsync();

                return Ok("Estudiante eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
