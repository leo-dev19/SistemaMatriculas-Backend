using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ProyectoDSWI.Models;
namespace servicio.Models
{
    public class GradoSeccion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } // Ejemplo: "5to Primaria - A"

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonIgnore]

        // Relación con Estudiantes
        public ICollection<Student> Estudiantes { get; set; } = new List<Student>();
        [JsonIgnore]

        // Relación con AsignacionesDocente
        public ICollection<AsignacionDocente> Asignaciones { get; set; } = new List<AsignacionDocente>();

        // Relación con Horario
        [JsonIgnore]
        public ICollection<Horario> Horarios { get; set; } = new List<Horario>();
    }
}
