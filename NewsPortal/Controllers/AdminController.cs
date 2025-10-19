using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.HelpModel;
using NewsPortal.Models;
using NewsPortal.Services.Interfaces;
using System.Security.Claims;

namespace NewsPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly INewsService _newsService;
        private readonly IValidator<News> _newsValidator;
        private readonly IPasswordHasher<Admin> _passwordHasher;

        public AdminController(IAdminService adminService, INewsService newsService,
            IValidator<News> newsValidator, IPasswordHasher<Admin> passwordHasher)
        {
            _adminService = adminService;
            _newsService = newsService;
            _newsValidator = newsValidator;
            _passwordHasher = passwordHasher;
        }

        // Доступна анонимно
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _adminService.GetByEmailAsync(model.Email);
            if (admin == null)
            {
                ViewBag.Error = "Неверный email или пароль";
                return View(model);
            }

            var result = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Неверный email или пароль";
                return View(model);
            }

            // Создание Claims с ролью Admin
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "AdminCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AdminCookie", principal);

            return RedirectToAction("Index");
        }

        // Только для авторизованных админов
        [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookie")]
        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetAllNewsAsync();
            return View(newsList);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookie")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminCookie");
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookie")]
        [HttpGet]
        public IActionResult CreateNews()
        {
            return View();
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookie")]
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

        [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookie")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var newsItem = await _newsService.GetNewsByIdAsync(id);
            if (newsItem == null)
                return NotFound();

            return View(newsItem);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookie")]
        [HttpPost]
        public async Task<IActionResult> Edit(News model)
        {
            var validationResult = await _newsValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                return View(model);
            }

            await _newsService.UpdateNewsAsync(model);
            return RedirectToAction("Index");
        }
    }
}
