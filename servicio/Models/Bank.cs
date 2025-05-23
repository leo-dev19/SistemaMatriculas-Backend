using System.ComponentModel.DataAnnotations;

namespace ProyectoDSWI.Models
{
    public class Bank
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del banco es obligatorio.")]
        public string BankName { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        //Relationship whit the table Payment
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
