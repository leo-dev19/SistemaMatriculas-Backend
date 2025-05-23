namespace ProyectoDSWI.Models
{
    public class LegalGuardian
    {
        public int Id { get; set; }
        public string IdentityDocument { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string CellphoneNumber { get; set; }
        public string Email { get; set; }
        public string Direction { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
