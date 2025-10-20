using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.HelpModel;
using NewsPortal.Models;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookie")]
    public class AdminController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IValidator<News> _newsValidator;
        private readonly IAuthenticationService _authService;

        public AdminController(INewsService newsService,
                               IValidator<News> newsValidator,
                               IAuthenticationService authService)
        {
            _newsService = newsService;
            _newsValidator = newsValidator;
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _authService.SignInAdminAsync(HttpContext, model.Email, model.Password);
            if (!success)
            {
                ModelState.AddModelError("", "Неверный email или пароль");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAdminAsync(HttpContext);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetAllNewsAsync();
            return View(newsList);
        }

        [HttpGet]
        public IActionResult CreateNews() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNews(News news)
        {
            if (!await ValidateNewsAsync(news))
                return View(news);

            await _newsService.CreateNewsAsync(news);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var newsItem = await _newsService.GetNewsByIdAsync(id);
            return newsItem == null ? NotFound() : View(newsItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(News news)
        {
            if (!await ValidateNewsAsync(news))
                return View(news);

            await _newsService.UpdateNewsAsync(news);
            return RedirectToAction("Index");
        }

        private async Task<bool> ValidateNewsAsync(News news)
        {
            var validationResult = await _newsValidator.ValidateAsync(news);
            if (validationResult.IsValid) return true;

            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return false;
        }
    }
}
