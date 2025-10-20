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
                news.ImageUrl = await SaveImageAsync(news.ImageFile);
            }

            news.CreatedAt = DateTime.UtcNow;
            news.UpdatedAt = DateTime.UtcNow;

            await _newsRepository.AddNews(news);
            return news;
        }

        public async Task UpdateNewsAsync(News news)
        {
            var existingNews = await _newsRepository.GetNewsById(news.Id);
            if (existingNews == null)
                throw new Exception("Новость не найдена");

            existingNews.Title = news.Title;
            existingNews.Subtitle = news.Subtitle;
            existingNews.Text = news.Text;
            existingNews.UpdatedAt = DateTime.UtcNow;

            if (news.ImageFile != null && news.ImageFile.Length > 0)
            {
                // Удаляем старое изображение
                if (!string.IsNullOrEmpty(existingNews.ImageUrl))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingNews.ImageUrl.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                existingNews.ImageUrl = await SaveImageAsync(news.ImageFile);
            }

            await _newsRepository.UpdateNews(existingNews);
        }

        public async Task DeleteNewsAsync(int id)
        {
            var newsItem = await _newsRepository.GetNewsById(id);
            if (newsItem == null)
                throw new Exception("Новость не найдена");

            if (!string.IsNullOrEmpty(newsItem.ImageUrl))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, newsItem.ImageUrl.TrimStart('/'));
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            await _newsRepository.DeleteNews(id);
        }


        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/news");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return "/images/news/" + fileName;
        }
        

    }
}
