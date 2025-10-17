using NewsPortal.Models;
using NewsPortal.Repositories.Interfaces;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(INewsRepository newsRepository) 
        { 
            _newsRepository = newsRepository;
        }

        public Task<IList<News>> GetAllNewsAsync()
        {
            return _newsRepository.GetAllNews();
        }

        public Task<News> GetNewsByIdAsync(int id)
        {
            return _newsRepository.GetNewsById(id);
        }

        public Task<News> AddNewsAsync(News news)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNewsAsync(News news)
        {
            return _newsRepository.UpdateNews(news);
        }

        public Task DeleteNewsAsync(int id)
        {
            return _newsRepository.DeleteNews(id);
        }

    }
}
