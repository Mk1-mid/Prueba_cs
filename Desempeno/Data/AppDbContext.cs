using Microsoft.EntityFrameworkCore;
using Desempeno.models;

namespace Desempeno.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<SportSpace> SportSpaces { get; set; }
        public DbSet<Reserv> Reservas { get; set; }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reserv>()
                .HasOne(r => r.usuario)
                .WithMany(u => u.reservas)
                .HasForeignKey(r => r.IdUser)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Reserv>()
                .HasOne(r => r.sportSpace)
                .WithMany(s => s.reservas)
                .HasForeignKey(r => r.IdSpace)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. MUY IMPORTANTE: Configuración del Enum Status
            // Esto asegura que en la base de datos se guarde el texto (string)
            // en lugar de un número (0, 1, 2)
            modelBuilder.Entity<Reserv>()
                .Property(r => r.Status)
                .HasConversion<string>();
        }
    }
}