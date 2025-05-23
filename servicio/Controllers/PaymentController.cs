using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using servicio.Data;
using servicio.Models.ModelsDTO;
using ProyectoDSWI.Models;

namespace servicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly MyAppContext myAppContext;

        public PaymentController(MyAppContext myAppContext)
        {
            this.myAppContext = myAppContext;
        }

        [HttpGet]
        public IActionResult GetAllPayments()
        {
            var payments = myAppContext.Payments.Where(p => p.Status).ToList();
            return Ok(payments);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = myAppContext.Payments.FirstOrDefault(p => p.Id == id && p.Status);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public IActionResult AddPayment(PaymentDTO paymentDTO)
        {
            var payment = new Payment
            {
                StudentId = paymentDTO.StudentId,
                PaymentTypeId = paymentDTO.PaymentTypeId,
                BankId = paymentDTO.BankId,
                Amount = paymentDTO.Amount,
                OperationCode = paymentDTO.OperationCode,
                PaymentStatusId = paymentDTO.PaymentStatusId,
                Status = true,
                RegistrationDate = DateTime.Now
            };

            myAppContext.Payments.Add(payment);
            myAppContext.SaveChanges();
            return Ok(payment);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdatePayment(int id, PaymentDTO paymentDTO)
        {
            var payment = myAppContext.Payments.Find(id);
            if (payment == null || !payment.Status)
                return NotFound();

            payment.StudentId = paymentDTO.StudentId;
            payment.PaymentTypeId = paymentDTO.PaymentTypeId;
            payment.BankId = paymentDTO.BankId;
            payment.Amount = paymentDTO.Amount;
            payment.OperationCode = paymentDTO.OperationCode;
            payment.PaymentStatusId = paymentDTO.PaymentStatusId;

            myAppContext.SaveChanges();
            return Ok(payment);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePayment(int id)
        {
            var payment = myAppContext.Payments.Find(id);
            if (payment == null || !payment.Status)
                return NotFound();

            payment.Status = false; // Eliminación lógica
            myAppContext.SaveChanges();
            return Ok();
        }
    }
}
