using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;
using NewsPortal.Repositories.Interfaces;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Services.Implementations
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NewsService(INewsRepository newsRepository,IWebHostEnvironment webHostEnvironment) 
        { 
            _newsRepository = newsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IList<News>> GetAllNewsAsync()
        {
            return await _newsRepository.GetAllNews();
        }

        public async Task<News> GetNewsByIdAsync(int id)
        {
            return await _newsRepository.GetNewsById(id);
        }

        public async Task<News> CreateNewsAsync(News news)
        {
            if (news.ImageFile != null && news.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(news.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await news.ImageFile.CopyToAsync(stream);

                news.ImageUrl = "/images/" + fileName;
            }

            news.CreatedAt = DateTime.UtcNow;
            await _newsRepository.AddNews(news);

            return news;
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
