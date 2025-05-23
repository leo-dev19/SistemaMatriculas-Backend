namespace servicio.Models.ModelsDTO
{
    public class MonthlyGenderStudentCountDTO
    {
        public int Total { get; set; }
        public List<MonthlyCountDTO>? MaleStudentCount { get; set; }
        public List<MonthlyCountDTO>? FemaleStudentCount { get; set; }
    }
}
