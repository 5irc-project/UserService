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


    }
}
