using System.ComponentModel.DataAnnotations;

namespace servicio.Models.ModelsDTO
{
    public class StudentDTO
    {
        [Required(ErrorMessage = "El código del estudiante es obligatorio.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El nombre del estudiante es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido del estudiante es obligatorio.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "La dirección del estudiante es obligatoria.")]
        public string Direction { get; set; }

        [Required(ErrorMessage = "El género del estudiante es obligatoria.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento del estudiante es obligatoria.")]
        public DateTime Birthdate { get; set; }
        public int? legalGuardianId { get; set; }
        public LegalGuardianDTO? LegalGuardian { get; set; }
        public IFormFile Imagen { get; set; }

    }
}
