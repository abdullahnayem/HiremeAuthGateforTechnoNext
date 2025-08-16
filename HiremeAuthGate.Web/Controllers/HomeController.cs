using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiremeAuthGate.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Displays the success page for authenticated users.
        /// Requires authorization to access.
        /// </summary>
        /// <returns>The success view with authentication success message.</returns>
        [Authorize]
        public IActionResult Success()
        {
            var userEmail = User.Identity?.Name;
            _logger.LogInformation("Success page accessed by user: {Email}", userEmail);
            return View();
        }

        /// <summary>
        /// Displays the error page for authentication failures.
        /// Shows error messages from TempData or default error message.
        /// </summary>
        /// <returns>The error view with authentication error message.</returns>
        public IActionResult Error()
        {
            var errorMessage = TempData["Error"]?.ToString() ?? "Unknown error";
            var userEmail = User.Identity?.Name;
            _logger.LogWarning("Error page accessed. Error: {ErrorMessage}, User: {Email}", errorMessage, userEmail);
            return View();
        }
    }
}
