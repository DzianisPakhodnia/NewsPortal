using NewsPortal.Models;
using NewsPortal.Repositories.Interfaces;
using NewsPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace NewsPortal.Repositories.Implementations
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public NewsRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task AddNews(News news)
        {
            _applicationDbContext.News.Add(news);
            await _applicationDbContext.SaveChangesAsync(); 
        }

        public async Task DeleteNews(int id)
        {
            var news = await _applicationDbContext.News.FindAsync(id);

            if (news != null)
            {
                _applicationDbContext.News.Remove(news);
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IList<News>> GetAllNews()
        {
            return await _applicationDbContext.News.ToListAsync();
        }

        public async Task<News?> GetNewsById(int id)
        {
            return await _applicationDbContext.News.FindAsync(id);
        }

        

        public async Task UpdateNews(News news)
        {
            _applicationDbContext.News.Update(news);
            await _applicationDbContext.SaveChangesAsync();
        }
      
    }
}
