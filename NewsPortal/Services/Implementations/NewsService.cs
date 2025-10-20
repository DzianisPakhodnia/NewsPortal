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
        private readonly string _uploadsFolder;

        public NewsService(INewsRepository newsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _newsRepository = newsRepository;
            _webHostEnvironment = webHostEnvironment;
            _uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/news");

            if (!Directory.Exists(_uploadsFolder))
                Directory.CreateDirectory(_uploadsFolder);
        }

        public async Task<IList<News>> GetAllNewsAsync()
        {
            var news = await _newsRepository.GetAllNews();
            return news.OrderByDescending(n => n.CreatedAt).ToList();
        }

        public async Task<News> GetNewsByIdAsync(int id)
        {
            var news = await _newsRepository.GetNewsById(id);
            if (news == null) throw new KeyNotFoundException("Новость не найдена");
            return news;
        }

        public async Task<News> CreateNewsAsync(News news)
        {
            news.CreatedAt = DateTime.UtcNow;
            news.UpdatedAt = DateTime.UtcNow;
            news.ImageUrl = await SaveOrUpdateImageAsync(news.ImageFile, null);

            await _newsRepository.AddNews(news);
            return news;
        }

        public async Task UpdateNewsAsync(News news)
        {
            var existingNews = await GetNewsByIdAsync(news.Id);

            existingNews.Title = news.Title;
            existingNews.Subtitle = news.Subtitle;
            existingNews.Text = news.Text;
            existingNews.UpdatedAt = DateTime.UtcNow;

            existingNews.ImageUrl = await SaveOrUpdateImageAsync(news.ImageFile, existingNews.ImageUrl);

            await _newsRepository.UpdateNews(existingNews);
        }

        public async Task DeleteNewsAsync(int id)
        {
            var news = await GetNewsByIdAsync(id);

            await DeleteImageAsync(news.ImageUrl);
            await _newsRepository.DeleteNews(id);
        }


        private async Task<string?> SaveOrUpdateImageAsync(IFormFile? imageFile, string? oldImageUrl)
        {
            if (imageFile == null || imageFile.Length == 0)
                return oldImageUrl;

            if (!string.IsNullOrEmpty(oldImageUrl))
                await DeleteImageAsync(oldImageUrl);

            return await SaveImageAsync(imageFile);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(_uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"/images/news/{fileName}";
        }

        private async Task DeleteImageAsync(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
                if (File.Exists(filePath))
                    await Task.Run(() => File.Delete(filePath));
            }
            catch
            {
                
            }
        }

    }
}
