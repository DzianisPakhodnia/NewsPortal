using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly INewsService _newsService;

        public AdminController(IAdminService adminService, INewsService newsService)
        {
            _adminService = adminService;
            _newsService = newsService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email и пароль обязательны";
                return View();
            }

            var admin = await _adminService.GetByEmailAsync(email);

            if (admin == null || admin.PasswordHash != password)
            {
                ViewBag.Error = "Неверный email или пароль";
                return View();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetAllNewsAsync();
            return View(newsList);
        }

        

        
        [HttpPost]
        public async Task<IActionResult> CreateNews(News news)
        {
            if (!ModelState.IsValid)
                return View(news);

            await _newsService.CreateNewsAsync(news);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateNews()
        {
            return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var newsItem = await _newsService.GetNewsByIdAsync(id);
            if (newsItem == null)
                return NotFound();

            return View(newsItem); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(News news)
        {
            if (!ModelState.IsValid)
                return View(news);

            var existingNews = await _newsService.GetNewsByIdAsync(news.Id);
            if (existingNews == null)
                return NotFound();

            existingNews.Title = news.Title;
            existingNews.Subtitle = news.Subtitle;
            existingNews.Text = news.Text;

            if (news.ImageFile != null && news.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(news.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await news.ImageFile.CopyToAsync(stream);
                }

                existingNews.ImageUrl = "/uploads/" + uniqueFileName;
            }

            await _newsService.UpdateNewsAsync(existingNews);

            return RedirectToAction("Index");
        }


    }
}
