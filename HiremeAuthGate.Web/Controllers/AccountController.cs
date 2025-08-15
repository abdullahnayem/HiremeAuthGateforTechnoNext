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
        public AccountController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginRequest());
        }
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

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterRequest());
        }

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

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


    }
}

