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

        public NewsService(INewsRepository newsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _newsRepository = newsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IList<News>> GetAllNewsAsync() => await _newsRepository.GetAllNews();

        public async Task<News> GetNewsByIdAsync(int id) => await _newsRepository.GetNewsById(id);

        public async Task<News> CreateNewsAsync(News news)
        {
            news.ImageUrl = await HandleImageAsync(news.ImageFile);
            news.CreatedAt = DateTime.UtcNow;
            news.UpdatedAt = DateTime.UtcNow;

            await _newsRepository.AddNews(news);
            return news;
        }

        public async Task UpdateNewsAsync(News news)
        {
            var existingNews = await _newsRepository.GetNewsById(news.Id)
                               ?? throw new KeyNotFoundException("Новость не найдена");

            existingNews.Title = news.Title;
            existingNews.Subtitle = news.Subtitle;
            existingNews.Text = news.Text;
            existingNews.UpdatedAt = DateTime.UtcNow;

            existingNews.ImageUrl = await HandleImageAsync(news.ImageFile, existingNews.ImageUrl);

            await _newsRepository.UpdateNews(existingNews);
        }

        public async Task DeleteNewsAsync(int id)
        {
            var newsItem = await _newsRepository.GetNewsById(id)
                           ?? throw new KeyNotFoundException("Новость не найдена");

            DeleteImage(newsItem.ImageUrl);
            await _newsRepository.DeleteNews(id);
        }

        private async Task<string?> HandleImageAsync(IFormFile? imageFile, string? oldImageUrl = null)
        {
            if (!string.IsNullOrEmpty(oldImageUrl))
                DeleteImage(oldImageUrl);

            if (imageFile == null || imageFile.Length == 0)
                return oldImageUrl;

            return await SaveImageAsync(imageFile);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/news");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return "/images/news/" + fileName;
        }

        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

    }
}
