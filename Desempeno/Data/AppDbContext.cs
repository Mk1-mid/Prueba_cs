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

            // Unique index for Users.Document
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Document)
                .IsUnique();

            modelBuilder.Entity<Reserv>()
                .HasOne(r => r.usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.IdUser)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Reserv>()
                .HasOne(r => r.sportSpace)
                .WithMany(s => s.Reservas)
                .HasForeignKey(r => r.IdSpace)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
