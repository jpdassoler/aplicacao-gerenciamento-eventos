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

        //DBSet para a entidade Evento
        public DbSet<Evento> Eventos { get; set; }

        //DBSet para a entidade ClienteEvento
        public DbSet<ClienteEvento> ClienteEventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configuração da conversão de enum para char
            modelBuilder.Entity<ClienteEvento>()
                .Property(e => e.Ind_Comparecimento)
                .HasConversion(
                    v => v.ToString()[0],  // Enum to char
                    v => (EnumIndComparecimento)v);  // char to Enum

            base.OnModelCreating(modelBuilder);
        }
    }
}
