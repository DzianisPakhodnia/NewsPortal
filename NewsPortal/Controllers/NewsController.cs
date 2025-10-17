using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;


        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetAllNewsAsync();
            return View(newsList);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveNews(int id)
        {
           
            await _newsService.DeleteNewsAsync(id);

            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNews(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateNews(News news)
        {
            var existingNews = await _newsService.GetNewsByIdAsync(news.Id);

            if (existingNews == null)
            {
                return NotFound();
            }

            existingNews.Title = news.Title;
            existingNews.Subtitle = news.Subtitle;
            existingNews.Text = news.Text;
            existingNews.ImageUrl = news.ImageUrl;
            await _newsService.UpdateNewsAsync(existingNews);

            return RedirectToAction("Index");

        }

    }
}
