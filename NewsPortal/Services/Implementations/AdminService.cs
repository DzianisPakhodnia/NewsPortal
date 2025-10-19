using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ValidateAdminAsync(string email, string password)
        {
            var admin = await _adminRepository.GetByEmailAsync(email);
            if (admin == null) return false;

            var result = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password);
            return result != PasswordVerificationResult.Failed;
        }

    }
}
