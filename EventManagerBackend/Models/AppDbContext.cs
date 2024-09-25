using Microsoft.EntityFrameworkCore;

namespace EventManagerBackend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //DbSet para a entidade Cliente
        public DbSet<Cliente> Clientes { get; set; }

        //DBSet para a entidade Endereco
        public DbSet<Endereco> Enderecos { get; set; }
    }
}
