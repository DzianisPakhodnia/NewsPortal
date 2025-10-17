using NewsPortal.Models;
using NewsPortal.Repositories.Interfaces;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository) 
        { 
            _adminRepository = adminRepository;
        }

        public async Task AddAsync(Admin admin)
        {
            await _adminRepository.AddAsync(admin);
        }

        public async Task DeleteAsync(int id)
        {
            await _adminRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _adminRepository.GetAllAsync();
        }

        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await _adminRepository.GetByEmailAsync(email);
        }

        public async Task<Admin> GetByIdAsync(int id)
        {
            return await _adminRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Admin admin)
        {
            await _adminRepository.UpdateAsync(admin);
        }
    }
}
