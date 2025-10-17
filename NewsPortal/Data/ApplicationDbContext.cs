using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;
namespace NewsPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }

        public DbSet<Admin> Admins { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }



    }
}
