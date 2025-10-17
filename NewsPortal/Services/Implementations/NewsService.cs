using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;
using NewsPortal.Repositories.Interfaces;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Services.Implementations
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(INewsRepository newsRepository) 
        { 
            _newsRepository = newsRepository;
        }

        public async Task<IList<News>> GetAllNewsAsync()
        {
            return await _newsRepository.GetAllNews();
        }

        public async Task<News> GetNewsByIdAsync(int id)
        {
            return await _newsRepository.GetNewsById(id);
        }

        public async Task<News> AddNewsAsync(News news)
        {
            throw new NotImplementedException();
        }

        public  Task UpdateNewsAsync(News news)
        {
            return _newsRepository.UpdateNews(news);
        }

        public Task DeleteNewsAsync(int id)
        {
            return _newsRepository.DeleteNews(id);
        }

    }
}
