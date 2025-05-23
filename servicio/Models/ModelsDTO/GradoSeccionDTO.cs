using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace servicio.Models.ModelsDTO
{
    public class GradoSeccionDTO
    {
        public int Id { get; set; } // Opcional para respuestas

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } // Ejemplo: "5to Primaria - A"
        [JsonIgnore]
        public List<HorarioDTO> Horarios { get; set; } = new List<HorarioDTO>();
    }
}
