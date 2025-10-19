using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly INewsService _newsService;
        private readonly IValidator<News> _newsValidator;
        private readonly IValidator<Admin> _adminValidator;

        public AdminController(IAdminService adminService, INewsService newsService, 
            IValidator<News> newsValidator, IValidator<Admin> adminValidator)
        {
            _adminService = adminService;
            _newsService = newsService;
            _newsValidator = newsValidator;
            _adminValidator = adminValidator;
        }
        // GET: /Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            var validationResult = await _adminValidator.ValidateAsync(admin);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(admin);
            }


            if (string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.PasswordHash))
            {
                ViewBag.Error = "Email и пароль обязательны";
                return View();
            }

            var existingAdmin = await _adminService.GetByEmailAsync(admin.Email);

            if (existingAdmin == null || existingAdmin.PasswordHash != admin.PasswordHash)
            {
                ViewBag.Error = "Неверный email или пароль";
                return View(admin);
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
            var validationResult = await _newsValidator.ValidateAsync(news);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                return View(news);
            }

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
        public async Task<IActionResult> Edit(News model)
        {
            var validationResult = await _newsValidator.ValidateAsync(model);

            if (!validationResult.IsValid) 
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                
                return View(model);
            }

            await _newsService.UpdateNewsAsync(model);

            return RedirectToAction("Index", "Admin");
        }

        


        



    }
}
