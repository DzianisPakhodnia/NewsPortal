using NewsPortal.Models;

namespace NewsPortal.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Admin> GetByIdAsync(int id);
        Task<Admin> GetByEmailAsync(string email);
        Task<IEnumerable<Admin>> GetAllAsync();
        Task AddAsync(Admin admin);
        Task UpdateAsync(Admin admin);
        Task DeleteAsync(int id);

        Task<Admin?> ValidateAdminAsync(string email, string password);
    }
}
