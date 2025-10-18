using NewsPortal.Models;

namespace NewsPortal.Services.Interfaces
{
    public interface INewsService
    {
        Task<IList<News>> GetAllNewsAsync();
        Task<News> GetNewsByIdAsync(int id);
        Task<News> CreateNewsAsync(News news);
        Task UpdateNewsAsync(News news);
        Task DeleteNewsAsync(int id);

    }
}
