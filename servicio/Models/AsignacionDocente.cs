using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace servicio.Models
{
    public class AsignacionDocente
    {
        [Key]
        public int Id { get; set; }

        public int DocenteId { get; set; }
        [ForeignKey("DocenteId")]
        public Docente Docente { get; set; }

        public int GradoSeccionId { get; set; }
        [ForeignKey("GradoSeccionId")]
        public GradoSeccion GradoSeccion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public ICollection<Horario> Horarios { get; set; } = new List<Horario>();
    }
}
