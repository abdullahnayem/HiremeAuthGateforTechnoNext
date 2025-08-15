using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiremeAuthGate.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public IActionResult Success()
        {
            ViewBag.Message = "Authentication Success";
            return View();
        }

        public IActionResult Error()
        {
            ViewBag.Message = "Authentication Error";
            return View();
        }
    }
}
