using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDSWI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int PaymentTypeId { get; set; }
        public int BankId { get; set; }
        public decimal Amount { get; set; }
        public string OperationCode { get; set; }
        public int PaymentStatusId { get; set; }
        public bool Status { get; set; } = true;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;


        //Relations
        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentType { get; set; }
        [ForeignKey("BankId")]
        public Bank Bank { get; set; }
        [ForeignKey("PaymentStatusId")]
        public PaymentStatus PaymentStatus { get; set; }

        //Relationship whit the table Quota
        public ICollection<Quota> Quotas { get; set; }
    }
}
