using System.Text.Json.Serialization;

namespace servicio.Models.ModelsDTO
{
    public class AsignacionDocenteDTO
    {
        public int Id { get; set; }
        public int DocenteId { get; set; }
        public int GradoSeccionId { get; set; }

        [JsonIgnore]
        public List<HorarioDTO> Horarios { get; set; } = new List<HorarioDTO>();
    }
}
