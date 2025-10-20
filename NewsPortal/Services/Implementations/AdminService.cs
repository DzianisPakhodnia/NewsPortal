using Microsoft.AspNetCore.Identity;
using NewsPortal.Models;
using NewsPortal.Repositories.Interfaces;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPasswordHasher<Admin> _passwordHasher;

        public AdminService(IAdminRepository adminRepository, IPasswordHasher<Admin> passwordHasher)
        {
            _adminRepository = adminRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task AddAsync(Admin admin) => await _adminRepository.AddAsync(admin);

        public async Task DeleteAsync(int id)
        {
            var admin = await GetByIdAsync(id);
            await _adminRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Admin>> GetAllAsync() => await _adminRepository.GetAllAsync();

        public async Task<Admin> GetByEmailAsync(string email)
        {
            var admin = await _adminRepository.GetByEmailAsync(email);
            if (admin == null)
                throw new KeyNotFoundException("Администратор с таким email не найден");
            return admin;
        }

        public async Task<Admin> GetByIdAsync(int id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
                throw new KeyNotFoundException("Администратор не найден");
            return admin;
        }

        public async Task UpdateAsync(Admin admin)
        {
            var existingAdmin = await GetByIdAsync(admin.Id);
            await _adminRepository.UpdateAsync(admin);
        }

        public async Task<Admin?> ValidateAdminAsync(string email, string password)
        {
            var admin = await _adminRepository.GetByEmailAsync(email);
            if (admin == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password);
            return result != PasswordVerificationResult.Failed ? admin : null;
        }
    }
}
