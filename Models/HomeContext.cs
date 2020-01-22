using Microsoft.EntityFrameworkCore;
 
namespace Prod_Cate.Models
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products {get;set;}
        public DbSet<Connection> Connections {get;set;}
        public DbSet<Category> Categories {get;set;}
    }
}