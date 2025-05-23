namespace servicio.Models.ModelsDTO
{
    public class QuotaDTO
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int QuotaStatus { get; set; }
    }
}
