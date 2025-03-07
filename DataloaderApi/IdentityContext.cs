using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataloaderApi.Data;

namespace DataloaderApi
{
    public class IdentityContext: IdentityDbContext<ApplicationUser>
    {

        public DbSet<TaskData> TaskData { get; set; }


        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuring many-to-many relationship
            builder.Entity<TaskData>()
                .HasMany(t => t.AssignedUsers)
                .WithMany(u => u.Tasks)
                .UsingEntity(j => j.ToTable("TaskDataUserJunction"));
        }

    }
}
