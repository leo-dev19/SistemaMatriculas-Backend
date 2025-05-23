namespace servicio.Models.ModelsDTO
{
    public class PaymentDTO
    {
        public int StudentId { get; set; }
        public int PaymentTypeId { get; set; }
        public int BankId { get; set; }
        public decimal Amount { get; set; }
        public string OperationCode { get; set; }
        public int PaymentStatusId { get; set; }
    }
}
