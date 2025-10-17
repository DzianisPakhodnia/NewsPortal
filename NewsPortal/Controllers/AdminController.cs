using Microsoft.AspNetCore.Mvc;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {

            _adminService = adminService;
        }
        public IActionResult Index()
        {
            return View();
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
