using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDSWI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Direction { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? LegalGuardianId { get; set; }

        // Nueva propiedad para la imagen
        public string ImagenPath { get; set; }

        [ForeignKey("LegalGuardianId")]
        public LegalGuardian LegalGuardian { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<Quota> Quotas { get; set; }
    }
}
