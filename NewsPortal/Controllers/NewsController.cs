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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveNews(int id)
        {
            try
            {
                await _newsService.DeleteNewsAsync(id);
                return RedirectToAction("Index", "Admin");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var newsItem = await _newsService.GetNewsByIdAsync(id);
                return View(newsItem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNews(News news)
        {
            try
            {
                await _newsService.UpdateNewsAsync(news);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
