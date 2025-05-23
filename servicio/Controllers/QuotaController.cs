using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDSWI.Models;
using servicio.Data;
using servicio.Models.ModelsDTO;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotaController : ControllerBase
    { 
        private readonly MyAppContext myAppContext;

        public QuotaController(MyAppContext myAppContext)
        {
            this.myAppContext = myAppContext;
        }

        [HttpGet]
        public IActionResult GetAllQuotas()
        {
            var quotas = myAppContext.Quotas.Where(q => q.Status).ToList();
            return Ok(quotas);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetQuotaById(int id)
        {
            var quota = myAppContext.Quotas.FirstOrDefault(q => q.Id == id && q.Status);
            if (quota == null)
                return NotFound();
            return Ok(quota);
        }

        [HttpPost]
        public IActionResult AddQuota(QuotaDTO quotaDTO)
        {
            var quota = new Quota
            {
                PaymentId = quotaDTO.PaymentId,
                Amount = quotaDTO.Amount,
                ExpirationDate = quotaDTO.ExpirationDate,
                QuotaStatus = quotaDTO.QuotaStatus,
                Status = true
            };

            myAppContext.Quotas.Add(quota);
            myAppContext.SaveChanges();
            return Ok(quota);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateQuota(int id, QuotaDTO quotaDTO)
        {
            var quota = myAppContext.Quotas.Find(id);
            if (quota == null || !quota.Status)
                return NotFound();

            quota.PaymentId = quotaDTO.PaymentId;
            quota.Amount = quotaDTO.Amount;
            quota.ExpirationDate = quotaDTO.ExpirationDate;
            quota.QuotaStatus = quotaDTO.QuotaStatus;

            myAppContext.SaveChanges();
            return Ok(quota);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteQuota(int id)
        {
            var quota = myAppContext.Quotas.Find(id);
            if (quota == null || !quota.Status)
                return NotFound();

            quota.Status = false; // Eliminación lógica
            myAppContext.SaveChanges();
            return Ok();
        }
    }
}
