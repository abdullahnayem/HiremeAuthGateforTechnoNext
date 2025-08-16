using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiremeAuthGate.Web.Controllers
{
    /// <summary>
    /// Controller for handling home page and general application views.
    /// Manages success and error pages for authentication flow.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Default action that redirects to the login page.
        /// </summary>
        /// <returns>Redirect to the Account/Login action.</returns>
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Displays the success page for authenticated users.
        /// Requires authorization to access.
        /// </summary>
        /// <returns>The success view with authentication success message.</returns>
        [Authorize]
        public IActionResult Success()
        {
            ViewBag.Message = "Authentication Success";
            return View();
        }

        /// <summary>
        /// Displays the error page for authentication failures.
        /// Shows error messages from TempData or default error message.
        /// </summary>
        /// <returns>The error view with authentication error message.</returns>
        public IActionResult Error()
        {
            ViewBag.Message = "Authentication Error";
            return View();
        }
    }
}
