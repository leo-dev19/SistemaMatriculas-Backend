using System.ComponentModel.DataAnnotations;

namespace servicio.Models.ModelsDTO
{
    public class DocenteDTO
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [MaxLength(9, ErrorMessage = "El DNI no puede superar los 9 caracteres.")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [MaxLength(100, ErrorMessage = "La especialidad no puede superar los 100 caracteres.")]
        public string Especialidad { get; set; }

        public bool Estado { get; set; } = true; 
    }
}
