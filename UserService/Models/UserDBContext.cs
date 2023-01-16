using Microsoft.EntityFrameworkCore;
using UserService.Models.EntityFramework;

namespace UserService.Models
{
    public class UserDBContext : DbContext
    {

        public UserDBContext() { }
        public UserDBContext(DbContextOptions<UserDBContext> options)
            : base(options ) { }  

        public DbSet<User> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.UserId).HasName("pk_uti");
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique().HasDatabaseName("uq_utl_mail");
        }

    }
}
