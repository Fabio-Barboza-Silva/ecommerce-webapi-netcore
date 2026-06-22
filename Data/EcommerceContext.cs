using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Data
{
    // O ":" significa que a nossa classe herda os superpoderes do DbContext da Microsoft
    public class EcommerceContext : DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {
        }
        // Aqui nós dizemos: "EF, mapeie a nossa classe Cliente para a tabela Clientes do banco"
        public DbSet<Cliente> Clientes { get; set; }

        // E faça o mesmo para a tabela de Produtos
        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Venda> Vendas { get; set; }

        //// Esse método configura para qual servidor e banco o C# deve olhar
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Essa é a String de Conexão. Ela diz o Servidor, o Banco e que a segurança é integrada (Windows)
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-9ACACJJ;Database=EcommerceDB;Trusted_Connection=True;TrustServerCertificate=True;");
        //}
    }
}