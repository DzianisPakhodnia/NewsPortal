using Microsoft.EntityFrameworkCore;
using NewsPortal.Data;
using NewsPortal.Models;
using NewsPortal.Repositories.Interfaces;

namespace NewsPortal.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AdminRepository(ApplicationDbContext applicationDbContext) 
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task AddAsync(Admin admin)
        {
            await _applicationDbContext.Admins.AddAsync(admin);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var admin = await _applicationDbContext.Admins.FindAsync(id);
            if (admin != null)
            {
                _applicationDbContext.Admins.Remove(admin);
                await _applicationDbContext.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _applicationDbContext.Admins.ToListAsync();
        }

        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await _applicationDbContext.Admins
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Admin> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Admins.FindAsync(id);
        }


        public async Task UpdateAsync(Admin admin)
        {
            _applicationDbContext.Admins.Update(admin);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
