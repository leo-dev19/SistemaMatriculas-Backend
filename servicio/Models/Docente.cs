using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace servicio.Models
{
    public class Docente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; }

        [Required]
        [MaxLength(20)]
        public string Dni { get; set; }

        [Required]
        [MaxLength(100)]
        public string Especialidad { get; set; }

        public bool Estado { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relación con AsignaciónDocente
        [JsonIgnore]
        public ICollection<AsignacionDocente> Asignaciones { get; set; } = new List<AsignacionDocente>();
    }
    }