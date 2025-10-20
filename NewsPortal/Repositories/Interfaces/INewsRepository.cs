using NewsPortal.Models;

namespace NewsPortal.Repositories.Interfaces
{
    public interface INewsRepository 
    { 
        Task<IList<News>> GetAllNews();
        Task<News?> GetNewsById(int id);
        Task AddNews(News news);
        Task UpdateNews(News news);
        Task DeleteNews(int id);

    }
}
