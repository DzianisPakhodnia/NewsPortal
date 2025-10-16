using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;
namespace NewsPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }


    }
}
