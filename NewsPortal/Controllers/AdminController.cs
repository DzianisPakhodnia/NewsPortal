using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using NewsPortal.Services.Implementations;
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
        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetAllNewsAsync();
            return View(newsList);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
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
        [HttpPost]
        public async Task<IActionResult> CreateNews(News news)
        {
            if (!ModelState.IsValid)
                return View(news);

            if (news.ImageFile != null && news.ImageFile.Length > 0)
            {
                // создаём папку uploads, если её нет
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // генерируем уникальное имя файла
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(news.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await news.ImageFile.CopyToAsync(stream);
                }

                // сохраняем относительный путь (для отображения в <img>)
                news.ImageUrl = "/images/" + uniqueFileName;
            }

            await _newsService.AddNewsAsync(news);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateNews()
        {
            return View(); // ищет Views/Admin/CreateNews.cshtml
        }

    }
}
