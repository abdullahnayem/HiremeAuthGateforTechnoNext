using HiremeAuthGate.BusinessModel.DTOs;
using HiremeAuthGate.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace HiremeAuthGate.Web.Controllers
{
    /// <summary>
    /// Controller for handling user authentication operations.
    /// Manages login, registration, and logout functionality with security features.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// The authentication service for user operations.
        /// </summary>
        private readonly IAuthService _auth;
        
        /// <summary>
        /// Initializes a new instance of the AccountController.
        /// </summary>
        /// <param name="auth">The authentication service dependency.</param>
        public AccountController(IAuthService auth)
        {
            _auth = auth;
        }

        /// <summary>
        /// Displays the login form to the user.
        /// </summary>
        /// <param name="returnUrl">Optional URL to redirect after successful login.</param>
        /// <returns>The login view with an empty LoginRequest model.</returns>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginRequest());
        }
        
        /// <summary>
        /// Processes user login authentication.
        /// Validates credentials, handles account locking, and creates authentication cookies.
        /// </summary>
        /// <param name="model">The login request containing email and password.</param>
        /// <param name="returnUrl">Optional URL to redirect after successful login.</param>
        /// <returns>Redirect to success page, error page, or return URL.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _auth.AuthenticateAsync(model.Email, model.Password);
            if (!result.IsSuccess)
            {
                // Check if it's an account lock error
                if (result.ErrorMessage?.Contains("Account is locked") == true)
                {
                    // Extract remaining time from the error message
                    var timeMatch = Regex.Match(result.ErrorMessage, @"(\d+) minutes");
                    if (timeMatch.Success && int.TryParse(timeMatch.Groups[1].Value, out int minutes))
                    {
                        TempData["Error"] = $"Your account has been locked. {minutes} minutes later try again.";
                    }
                    else
                    {
                        TempData["Error"] = "Your account has been locked";
                    }
                    return RedirectToAction("Error", "Home");
                }
                
                // For other errors
                TempData["Error"] = "Invalid email or password";
                return RedirectToAction("Error", "Home");
            }

            var user = result.Value!;
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Email),
                new(ClaimTypes.Email, user.Email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Success", "Home");
        }

        /// <summary>
        /// Displays the registration form to the user.
        /// </summary>
        /// <returns>The registration view with an empty RegisterRequest model.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterRequest());
        }

        /// <summary>
        /// Processes user registration.
        /// Validates input and creates a new user account.
        /// </summary>
        /// <param name="model">The registration request containing email, password, and confirmation.</param>
        /// <returns>Redirect to login page on success, or registration view with errors.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _auth.RegisterAsync(model.Email, model.Password);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Registration failed. Please try again.");
                return View(model);
            }

            TempData["Success"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Logs out the currently authenticated user.
        /// Clears authentication cookies and redirects to login page.
        /// </summary>
        /// <returns>Redirect to login page.</returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}

