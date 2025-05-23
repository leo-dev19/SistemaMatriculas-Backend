namespace servicio.Models.ModelsDTO
{
    public class HorarioResponseDTO
    {
        public int Id { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string DiaSemana { get; set; }
        public int GradoSeccionId { get; set; }
        public string GradoSeccionNombre { get; set; }
    }
}
