using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDSWI.Models
{
    public class Quota
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int QuotaStatus { get; set; }
        public bool Status { get; set; } = true;


        //Relations
        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
    }
}
