namespace ProyectoDSWI.Models
{
    public class PaymentType
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; } = true;


        //Relationship whit the table Payment
        public ICollection<Payment> Payments { get; set; }
    }
}
