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

            var sortedNews = newsList.OrderByDescending(n => n.CreatedAt).ToList();

            return View(sortedNews);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveNews(int id)
        {
            var newsItem = await _newsService.GetNewsByIdAsync(id);
            if (newsItem == null)
                return NotFound();

            await _newsService.DeleteNewsAsync(id);

            return RedirectToAction("Index", "Admin");
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var newsItem = await _newsService.GetNewsByIdAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
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
