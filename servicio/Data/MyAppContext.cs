using Microsoft.EntityFrameworkCore;
using ProyectoDSWI.Models;
using servicio.Models;

namespace servicio.Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions options) : base(options)
        {
        }

        //Migration : "LoginUser"

        public DbSet<User> Users { get; set; }

        //Migration : "Students and Payments"
        public DbSet<Student> Students { get; set; }
        public DbSet<LegalGuardian> LegalGuardians { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<Quota> Quotas { get; set; }

        //Migration : "Matricula"
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<GradoSeccion> GradoSecciones { get; set; }
        public DbSet<AsignacionDocente> AsignacionesDocentes { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Horario>()
                .HasOne(h => h.GradoSeccion)
                .WithMany(g => g.Horarios)
                .HasForeignKey(h => h.GradoSeccionId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Matricula>()
               .HasOne(m => m.Student)
               .WithMany()  // Relación inversa, puedes agregar la relación inversa si existe
               .HasForeignKey(m => m.StudentId)
               .OnDelete(DeleteBehavior.Restrict);  // Cambiar a Restrict o NoAction

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Docente)
                .WithMany()  // Relación inversa, puedes agregar la relación inversa si existe
                .HasForeignKey(m => m.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);  // Cambiar a Restrict o NoAction

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Horario)
                .WithMany()  // Relación inversa, puedes agregar la relación inversa si existe
                .HasForeignKey(m => m.HorarioId)
                .OnDelete(DeleteBehavior.Restrict);  // Cambiar a Restrict o NoAction

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.GradoSeccion)
                .WithMany()  // Relación inversa, puedes agregar la relación inversa si existe
                .HasForeignKey(m => m.GradoSeccionId)
                .OnDelete(DeleteBehavior.Restrict);  // Cambiar a Restrict o NoAction

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.LegalGuardian)
                .WithMany()  // Relación inversa, puedes agregar la relación inversa si existe
                .HasForeignKey(m => m.LegalGuardianId)
                .OnDelete(DeleteBehavior.Restrict);  // Cambiar a Restrict o NoAction

        }
    }
}
