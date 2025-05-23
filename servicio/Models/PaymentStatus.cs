namespace ProyectoDSWI.Models
{
    public class PaymentStatus
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; } = true;


        //Relationship whit the table Payment and Quota
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Quota> Quotas { get; set; }
    }
}
