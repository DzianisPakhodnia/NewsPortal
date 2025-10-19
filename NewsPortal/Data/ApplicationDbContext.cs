using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;
namespace NewsPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<News> News { get; set; }

        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<News>(entity => 
            {
                entity.HasKey(n => n.Id);
                entity.Property(n=>n.Id).ValueGeneratedOnAdd();
                entity.Property(n => n.Title).IsRequired().HasMaxLength(200);
                entity.Property(n => n.Text).IsRequired();
                entity.Property(n => n.CreatedAt).HasDefaultValueSql("NOW()");
                entity.Property(n => n.UpdatedAt).HasDefaultValueSql("NOW()");
                entity.Ignore(n => n.ImageFile);


            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).ValueGeneratedOnAdd();
                entity.Property(a => a.Name).IsRequired();
                entity.Property(a => a.Email).IsRequired();
                entity.Property(a => a.PasswordHash).IsRequired();
            });

        }


    }
}
