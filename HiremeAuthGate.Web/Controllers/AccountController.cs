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
    public class AccountController : Controller
    {
        private readonly IAuthService _auth;

        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService auth, ILogger<AccountController> logger)
        {
            _auth = auth;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            _logger.LogInformation("Login page accessed. ReturnUrl: {ReturnUrl}", returnUrl);
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
            _logger.LogInformation("Login attempt for email: {Email}, ReturnUrl: {ReturnUrl}", model.Email, returnUrl);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Login failed: Model validation errors for email: {Email}", model.Email);
                    return View(model);
                }

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
                            _logger.LogWarning("Login failed: Account locked for email: {Email}, Remaining time: {Minutes} minutes",
                                model.Email, minutes);
                            TempData["Warning"] = $"Your account has been temporarily locked due to multiple failed attempts. Please try again in {minutes} minutes.";
                        }
                        else
                        {
                            _logger.LogWarning("Login failed: Account locked for email: {Email}", model.Email);
                            TempData["Warning"] = "Your account has been temporarily locked due to multiple failed attempts. Please try again later.";
                        }
                        return RedirectToAction("Login"); // Redirect to Login page to show message
                    }

                    // For other errors
                    _logger.LogWarning("Login failed: Invalid credentials for email: {Email}, Error: {ErrorMessage}",
                        model.Email, result.ErrorMessage);
                    TempData["Error"] = "Invalid email or password. Please check your credentials and try again.";
                    return RedirectToAction("Error", "Home");
                    // return RedirectToAction("Login"); // Redirect to Login page to show message
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

                _logger.LogInformation("Login successful for email: {Email}, UserId: {UserId}", model.Email, user.Id);
                TempData["Success"] = $"Welcome back, {user.Email}! You have been successfully logged in.";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    _logger.LogInformation("Redirecting user {Email} to return URL: {ReturnUrl}", model.Email, returnUrl);
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Success", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed with exception for email: {Email}", model.Email);
                TempData["Error"] = "An unexpected error occurred. Please try again.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Registration page accessed");
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
            _logger.LogInformation("Registration attempt for email: {Email}", model.Email);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Registration failed: Model validation errors for email: {Email}", model.Email);
                    return View(model);
                }

                var result = await _auth.RegisterAsync(model.Email, model.Password);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Registration failed for email: {Email}, Error: {ErrorMessage}",
                        model.Email, result.ErrorMessage);

                    // Check for specific error types
                    if (result.ErrorMessage?.Contains("already exists") == true)
                    {
                        TempData["Error"] = "An account with this email address already exists. Please use a different email or try logging in.";
                    }
                    else if (result.ErrorMessage?.Contains("password") == true)
                    {
                        TempData["Error"] = "Password does not meet the requirements. Please ensure your password is at least 8 characters long.";
                    }
                    else
                    {
                        TempData["Error"] = result.ErrorMessage ?? "Registration failed. Please check your information and try again.";
                    }
                    return RedirectToAction("Register"); // Redirect to Register page to show message
                }

                _logger.LogInformation("Registration successful for email: {Email}, UserId: {UserId}",
                    model.Email, result.Value?.Id);
                TempData["Success"] = "Registration successful! Your account has been created. Please login with your credentials.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed with exception for email: {Email}", model.Email);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(model);
            }
        }

        /// <summary>
        /// Logs out the currently authenticated user.
        /// Clears authentication cookies and redirects to login page.
        /// </summary>
        /// <returns>Redirect to login page.</returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userEmail = User.Identity?.Name;
                _logger.LogInformation("Logout initiated for user: {Email}", userEmail);

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("Logout successful for user: {Email}", userEmail);
                TempData["Success"] = "You have been successfully logged out. Thank you for using our service.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed with exception for user: {Email}", User.Identity?.Name);
                return RedirectToAction("Login");
            }
        }
    }
}

