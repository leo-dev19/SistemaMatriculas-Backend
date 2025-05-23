namespace servicio.Models.ModelsDTO
{
    public class MatriculaDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int DocenteId { get; set; }
        public int HorarioId { get; set; }
        public int GradoSeccionId { get; set; }
        public int? LegalGuardianId { get; set; }
        public DateTime FechaMatricula { get; set; }
        public bool Estado { get; set; }
    }
}
