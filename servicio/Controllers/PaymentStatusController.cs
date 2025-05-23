using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDSWI.Models;
using servicio.Data;
using servicio.Models.ModelsDTO;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentStatusController : ControllerBase
    {
        private readonly MyAppContext myAppContext;

        public PaymentStatusController(MyAppContext myAppContext)
        {
            this.myAppContext = myAppContext;
        }

        [HttpGet]
        public IActionResult GetAllPaymentStatuses()
        {
            var paymentStatuses = myAppContext.PaymentStatus.Where(ps => ps.Status).ToList();
            return Ok(paymentStatuses);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPaymentStatusById(int id)
        {
            var paymentStatus = myAppContext.PaymentStatus.FirstOrDefault(ps => ps.Id == id && ps.Status);
            if (paymentStatus == null)
                return NotFound();
            return Ok(paymentStatus);
        }

        [HttpPost]
        public IActionResult AddPaymentStatus(PaymentStatusDTO paymentStatusDTO)
        {
            var paymentStatus = new PaymentStatus
            {
                Description = paymentStatusDTO.Description,
                Status = true
            };

            myAppContext.PaymentStatus.Add(paymentStatus);
            myAppContext.SaveChanges();
            return Ok(paymentStatus);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdatePaymentStatus(int id, PaymentStatusDTO paymentStatusDTO)
        {
            var paymentStatus = myAppContext.PaymentStatus.Find(id);
            if (paymentStatus == null || !paymentStatus.Status)
                return NotFound();

            paymentStatus.Description = paymentStatusDTO.Description;

            myAppContext.SaveChanges();
            return Ok(paymentStatus);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePaymentStatus(int id)
        {
            var paymentStatus = myAppContext.PaymentStatus.Find(id);
            if (paymentStatus == null || !paymentStatus.Status)
                return NotFound();

            paymentStatus.Status = false; // Eliminación lógica
            myAppContext.SaveChanges();
            return Ok();
        }
    }
}
