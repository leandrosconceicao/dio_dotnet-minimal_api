using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Enums;

namespace minimal_api.Infraestrutura.Db
{
    public class ApiContext(DbContextOptions<ApiContext> opts, IConfiguration configuration) : DbContext(opts)
    {
        public DbSet<Administradores> Administradores { get; set; } = default!;
        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administradores>().HasData(
                new Administradores
                {
                    Id = 1,
                    Email = "administrador@teste.com.br",
                    Senha = "123456",
                    Perfil = Perfil.Adm.ToString()
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var stringConnection = configuration.GetConnectionString("SqlConnection")?.ToString();
            if (!string.IsNullOrEmpty(stringConnection) && !optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(
                    stringConnection,
                    opts => opts.MigrationsAssembly("minimal-api")
                );
            }
        }
    }
}
