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
                .WithMany()
                .HasForeignKey(r => r.IdUser)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Reserv>()
                .HasOne(r => r.sportSpace)
                .WithMany()
                .HasForeignKey(r => r.IdSpace)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
