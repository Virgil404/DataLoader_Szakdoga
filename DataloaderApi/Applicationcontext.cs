using DataloaderApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DataloaderApi
{
    public class Applicationcontext: DbContext

        
    {
      
        public Applicationcontext(DbContextOptions<Applicationcontext> options) : base(options)
        {
        }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<CrimeData> CrimeData { get; set; }
        public DbSet<Tables> Tables { get; set; }
    }
}
