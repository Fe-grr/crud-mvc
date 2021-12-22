using System.Data.Entity;

namespace Citel.WebApi.Models
{
    public class CitelContext : DbContext
    {
        public CitelContext() : base("CitelContexto") { }

        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
    }
}