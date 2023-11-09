using Microsoft.EntityFrameworkCore;
using QrDefinitivo.Models;

namespace QrDefinitivo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define los DbSet para tus entidades
        public DbSet<Etiqueta> Etiquetas { get; set; }
        // public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Puedes configurar aquí las relaciones y restricciones de tus entidades si es necesario
        }
    }
}
