using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDSWI.Models;
using servicio.Data;
using servicio.Models.ModelsDTO;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalGuardianController : ControllerBase
    {
        private readonly MyAppContext myAppContext;

        public LegalGuardianController(MyAppContext myAppContext)
        {
            this.myAppContext = myAppContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllLegalGuardians()
        {
            var allLegalGuardians = await myAppContext.LegalGuardians.ToListAsync();

            if (allLegalGuardians == null || !allLegalGuardians.Any())
            {
                return NotFound("No se encontraron apoderados.");
            }

            return Ok(allLegalGuardians);
        }

        [HttpGet("{dni}")]
        [Authorize]
        public async Task<IActionResult> GetLegalGuardianByDNI(string dni)
        {
            if (dni == null)
            {
                return BadRequest("Ingrese un DNI");
            }

            var existingLegalGuardian = await myAppContext.LegalGuardians.Where(lg => lg.IdentityDocument == dni).FirstOrDefaultAsync();

            if (existingLegalGuardian == null)
            {
                return BadRequest("No hay coincodencias encontradas.");
            }

            return Ok(existingLegalGuardian);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateLegalGuardian([FromBody] LegalGuardian newLegalGuardian)
        {
            if (newLegalGuardian == null)
            {
                return BadRequest("Los datos del apoderado son inválidos.");
            }

            using (var transaction = await myAppContext.Database.BeginTransactionAsync())
            {
                try
                {
                    myAppContext.LegalGuardians.Add(newLegalGuardian);
                    await myAppContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Ok(newLegalGuardian);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error al guardar el apoderado: {ex.Message}");
                }
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateLegalGuardian(int id, [FromBody] LegalGuardianDTO updatedLegalGuardian)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLegalGuardian = await myAppContext.LegalGuardians
                .FirstOrDefaultAsync(lg => lg.Id == id);

            if (existingLegalGuardian == null)
            {
                return NotFound(new { message = "El apoderado no fue encontrado." });
            }

            using (var transaction = await myAppContext.Database.BeginTransactionAsync())
            {
                try
                {
                    existingLegalGuardian.IdentityDocument = updatedLegalGuardian.IdentityDocument;
                    existingLegalGuardian.Name = updatedLegalGuardian.Name;
                    existingLegalGuardian.LastName = updatedLegalGuardian.LastName;
                    existingLegalGuardian.Gender = updatedLegalGuardian.Gender;
                    existingLegalGuardian.Birthdate = updatedLegalGuardian.Birthdate;
                    existingLegalGuardian.CellphoneNumber = updatedLegalGuardian.CellphoneNumber;
                    existingLegalGuardian.Email = updatedLegalGuardian.Email;
                    existingLegalGuardian.Direction = updatedLegalGuardian.Direction;

                    myAppContext.LegalGuardians.Update(existingLegalGuardian);
                    await myAppContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Ok(new { message = "Apoderado actualizado exitosamente" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error al actualizar el apoderado: {ex.Message}" });
                }
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLegalGuardian(int id)
        {
            var existingLegalGuardian = await myAppContext.LegalGuardians
                .FirstOrDefaultAsync(lg => lg.Id == id);

            if (existingLegalGuardian == null)
            {
                return NotFound("El apoderado no fue encontrado.");
            }

            using (var transaction = await myAppContext.Database.BeginTransactionAsync())
            {
                try
                {
                    myAppContext.LegalGuardians.Remove(existingLegalGuardian);
                    await myAppContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Ok("Apoderado eliminado exitosamente");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el apoderado: {ex.Message}");
                }
            }
        }
    }
}
