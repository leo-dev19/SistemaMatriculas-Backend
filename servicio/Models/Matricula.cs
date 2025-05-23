using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProyectoDSWI.Models;
using servicio.Models;

namespace servicio.Models
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }

        // Relación con Estudiante
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        // Relación con Docente
        public int DocenteId { get; set; }
        [ForeignKey("DocenteId")]
        public Docente Docente { get; set; }

        // Relación con Horario
        public int HorarioId { get; set; }
        [ForeignKey("HorarioId")]
        public Horario Horario { get; set; }

        // Relación con Grado y Sección
        public int GradoSeccionId { get; set; }
        [ForeignKey("GradoSeccionId")]
        public GradoSeccion GradoSeccion { get; set; }

        // Relación con Apoderado (LegalGuardian)
        public int? LegalGuardianId { get; set; }
        [ForeignKey("LegalGuardianId")]
        public LegalGuardian LegalGuardian { get; set; }

        
        // Fecha de la matrícula
        [Required]
        public DateTime FechaMatricula { get; set; } = DateTime.Now;

        // Estado de la matrícula (Activa/Inactiva)
        public bool Estado { get; set; } = true;
    }
}

