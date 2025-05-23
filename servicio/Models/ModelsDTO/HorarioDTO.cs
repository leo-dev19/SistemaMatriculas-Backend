namespace servicio.Models.ModelsDTO
{
    public class HorarioDTO
    {
        public int Id { get; set; }

        public string HoraInicio { get; set; }

        public string HoraFin { get; set; }

        public string DiaSemana { get; set; } // Lunes, Martes, etc.

        // Relación con GradoSeccion
        public int GradoSeccionId { get; set; }
    }
}
