using Microsoft.AspNetCore.Mvc;
using NewsPortal.Services.Interfaces;

namespace NewsPortal.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }





    }
}
