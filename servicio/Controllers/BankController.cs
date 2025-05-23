using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDSWI.Models;
using servicio.Data;
using servicio.Models.ModelsDTO;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly MyAppContext myAppContext;

        public BankController(MyAppContext myAppContext)
        {
            this.myAppContext = myAppContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllBanks()
        {
            var allBanks = myAppContext.Banks.ToList();
            return Ok(allBanks);
        }

        [HttpPost]
        [Authorize(Roles = ("Administrador"))]
        public IActionResult AddBank(BankDTO bankDTO)
        {
            using (var transaction = myAppContext.Database.BeginTransaction())
            {
                try
                {
                    var bank = new Bank()
                    {
                        BankName = bankDTO.BankName
                    };

                    myAppContext.Banks.Add(bank);
                    myAppContext.SaveChanges();

                    transaction.Commit();

                    return Ok(bank);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ("Administrador"))]
        public IActionResult EditBank(int id, [FromBody] BankDTO bankDTO)
        {
            using (var transaction = myAppContext.Database.BeginTransaction())
            {
                try
                {
                    var bank = myAppContext.Banks.FirstOrDefault(b => b.Id == id);

                    if (bank == null)
                    {
                        return NotFound($"Banco con ID {id} no encontrado");
                    }

                    bank.BankName = bankDTO.BankName;

                    myAppContext.SaveChanges();
                    transaction.Commit();

                    return Ok($"Banco con ID {id} ha sido actualizado.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpDelete("(id)")]
        [Authorize(Roles = ("Administrador"))]
        public IActionResult DeleteBank(int id)
        {
            using (var transaction = myAppContext.Database.BeginTransaction())
            {
                try
                {
                    var bank = myAppContext.Banks.FirstOrDefault(b => b.Id == id);

                    if (bank == null)
                    {
                        return NotFound($"Banco con ID {id} no encontrado");
                    }

                    myAppContext.Banks.Remove(bank);
                    myAppContext.SaveChanges();

                    transaction.Commit();

                    return Ok($"Banco con ID {id} ha sido eliminado");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpPut("desactivate/{id}")]
        [Authorize(Roles = ("Administrador"))]
        public IActionResult DesactivateBank(int id)
        {
            using (var transaction = myAppContext.Database.BeginTransaction())
            {
                try
                {
                    var bank = myAppContext.Banks.FirstOrDefault(b => b.Id == id);

                    if (bank == null)
                    {
                        return NotFound($"Banco con ID {id} no encontrado.");
                    }

                    bank.Status = false;

                    myAppContext.SaveChanges();
                    transaction.Commit();

                    return Ok($"Banco con ID {id} ha sido desactivado");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest($"Ocurrió un error: {ex.Message}");
                }
            }
        }

        [HttpPut("reinstate/{id}")]
        [Authorize(Roles = ("Administrador"))]
        public IActionResult ReinstateBank(int id)
        {
            using (var transaction = myAppContext.Database.BeginTransaction())
            {
                try
                {
                    var bank = myAppContext.Banks.FirstOrDefault(b => b.Id == id);

                    if (bank == null)
                    {
                        return NotFound($"Banco con ID {id} no encontrado.");
                    }

                    bank.Status = true;

                    myAppContext.SaveChanges();
                    transaction.Commit();

                    return Ok($"Banco con ID {id} ha sido reingresado (activo nuevamente).");
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
