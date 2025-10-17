using NewsPortal.Models;

namespace NewsPortal.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> GetByIdAsync(int id);
        Task<Admin> GetByEmailAsync(string email);
        Task<IEnumerable<Admin>> GetAllAsync();
        Task AddAsync(Admin admin);
        Task UpdateAsync(Admin admin);
        Task DeleteAsync(int id);

    }
}
