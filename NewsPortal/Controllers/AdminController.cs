using Microsoft.AspNetCore.Mvc;

namespace NewsPortal.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
