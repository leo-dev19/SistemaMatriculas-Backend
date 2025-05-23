using System.ComponentModel.DataAnnotations;

namespace servicio.Models.ModelsDTO
{
    public class LegalGuardianDTO
    {
        [Required(ErrorMessage = "El DNI del apoderado es obligatorio.")]
        public string IdentityDocument { get; set; }

        [Required(ErrorMessage = "El nombre del apoderado es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido del apoderado es obligatorio.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El género del apoderado es obligatorio.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento del apoderado es obligatoria.")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "El número de celular del apoderado es obligatorio.")]
        public string CellphoneNumber { get; set; }

        [Required(ErrorMessage = "El correo electrónico del apoderado es obligatorio.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La dirección del apoderado es obligatoria.")]
        public string Direction { get; set; }
    }
}
