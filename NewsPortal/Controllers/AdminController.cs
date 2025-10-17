using Microsoft.AspNetCore.Mvc;
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

    }
}
