namespace servicio.Models.ModelsDTO
{
    public class StatisticsDTO
    {
        public int Total { get; set; }
        public decimal PercentageChange { get; set; }
        public List<MonthlyCountDTO>? MonthlyCounts { get; set; }
    }
}
