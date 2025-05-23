using System.ComponentModel.DataAnnotations;

namespace servicio.Models.ModelsDTO
{
    public class BankDTO
    {
        [Required(ErrorMessage = "El nombre del banco es obligatorio.")]
        public required string BankName { get; set; }
    }
}
