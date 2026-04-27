using Microsoft.EntityFrameworkCore;
using Desempeno.models;
namespace Desempeno.services;

public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Users> users { get; set; }
    public DbSet<SportSpace>  sport_spaces { get; set; }
    public DbSet<Reserv> reservas { get; set; }
     
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Reserv>()
            .HasOne(r => r.usuario)
            .WithMany(u => u.reservas)
            .HasForeignKey(r => r.IdUser);

        modelBuilder.Entity<Reserv>()
            .HasOne(r => r.sportSpace)
            .WithMany(s => s.reservas)
            .HasForeignKey(r => r.IdSpace);
            
            
    
}