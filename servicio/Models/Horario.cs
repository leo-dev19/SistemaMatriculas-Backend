using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace servicio.Models
{
    public class Horario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public TimeSpan HoraFin { get; set; }

        [Required]
        public string DiaSemana { get; set; } // Lunes, Martes, etc.

        // Relación con GradoSeccion
        public int GradoSeccionId { get; set; }
        [ForeignKey("GradoSeccionId")]
        public GradoSeccion GradoSeccion { get; set; }
    }
}