using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDSWI.Models;
using servicio.Data;
using servicio.Models.ModelsDTO;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    { 
        private readonly MyAppContext myAppContext;

        public PaymentTypeController(MyAppContext myAppContext)
        {
            this.myAppContext = myAppContext;
        }

        [HttpGet]
        public IActionResult GetAllPaymentTypes()
        {
            var paymentTypes = myAppContext.PaymentTypes.Where(pt => pt.Status).ToList();
            return Ok(paymentTypes);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPaymentTypeById(int id)
        {
            var paymentType = myAppContext.PaymentTypes.FirstOrDefault(pt => pt.Id == id && pt.Status);
            if (paymentType == null)
                return NotFound();
            return Ok(paymentType);
        }

        [HttpPost]
        public IActionResult AddPaymentType(PaymentTypeDTO paymentTypeDTO)
        {
            var paymentType = new PaymentType
            {
                Description = paymentTypeDTO.Description,
                Status = true
            };

            myAppContext.PaymentTypes.Add(paymentType);
            myAppContext.SaveChanges();
            return Ok(paymentType);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdatePaymentType(int id, PaymentTypeDTO paymentTypeDTO)
        {
            var paymentType = myAppContext.PaymentTypes.Find(id);
            if (paymentType == null || !paymentType.Status)
                return NotFound();

            paymentType.Description = paymentTypeDTO.Description;

            myAppContext.SaveChanges();
            return Ok(paymentType);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePaymentType(int id)
        {
            var paymentType = myAppContext.PaymentTypes.Find(id);
            if (paymentType == null || !paymentType.Status)
                return NotFound();

            paymentType.Status = false; // Eliminación lógica
            myAppContext.SaveChanges();
            return Ok();
        }
    }
}
