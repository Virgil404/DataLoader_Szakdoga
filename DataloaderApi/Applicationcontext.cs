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

     //   public DbSet<User> Users { get; set; }

       // public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(new User
            {
                UserID = "Admin",
                Password = "$2a$11$0WZaQI.t68SRP7sBnBKFYe5mKMgLuLDQ2CyI5FnZWhBFVO/xRO6qS",
                Role ="Admin"
              


            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.RefreshToken)    // User has one RefreshToken
                .WithOne(rt => rt.User)        // RefreshToken has one User
                .HasForeignKey<RefreshToken>(rt => rt.Username) // FK on RefreshToken
                .OnDelete(DeleteBehavior.Cascade);
            */
        }
            
    }
}
