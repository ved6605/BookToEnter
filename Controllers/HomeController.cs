using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Book2Enter.Models;
using Book2Enter.Services;

namespace Book2Enter.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUserService userService, ILogger<HomeController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // Login page (was Pages/Index)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // API: Login (AJAX)
        public class LoginRequest { public string UserId { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult LoginAjax([FromBody] LoginRequest req)
        {
            var user = _userService.GetByUserId(req.UserId);
            if (user == null) return Json(new { success = false, message = "Invalid User ID or Password" });

            var (ok, message) = _userService.ValidateCredentials(req.UserId, req.Password);
            if (!ok) return Json(new { success = false, message });

            if (user.IsFirstLogin)
            {
                _userService.GenerateOtp(user.UserId);
                return Json(new { success = true, requiresOtp = true, message = "OTP sent to registered email" });
            }

            var redirect = RoleToUrl(user.Role);
            return Json(new { success = true, requiresOtp = false, redirectUrl = redirect });
        }

        public class OtpRequest { public string UserId { get; set; } = string.Empty; public string Otp { get; set; } = string.Empty; public string Purpose { get; set; } = string.Empty; }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult SendOtp([FromBody] OtpRequest req)
        {
            var user = _userService.GetByUserId(req.UserId);
            if (user == null) return Json(new { success = false, message = "Unknown User ID" });
            _userService.GenerateOtp(user.UserId);
            return Json(new { success = true, message = "OTP sent to registered email" });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult VerifyOtp([FromBody] OtpRequest req)
        {
            var ok = _userService.VerifyOtp(req.UserId, req.Otp);
            if (!ok) return Json(new { success = false, message = "Invalid or expired OTP" });

            if (req.Purpose == "firstLogin")
            {
                return Json(new { success = true, requirePasswordChange = true, message = "OTP verified. Please change password." });
            }

            // For forgot purpose we allow reset
            return Json(new { success = true, allowReset = true, message = "OTP verified. You may reset your password." });
        }

        public class ChangePasswordRequest { public string UserId { get; set; } = string.Empty; public string NewPassword { get; set; } = string.Empty; }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest req)
        {
            var user = _userService.GetByUserId(req.UserId);
            if (user == null) return Json(new { success = false, message = "Unknown User ID" });

            _userService.SetPassword(req.UserId, req.NewPassword);
            var redirect = RoleToUrl(user.Role);
            return Json(new { success = true, redirectUrl = redirect, message = "Password changed" });
        }

        private string RoleToUrl(string role)
        {
            return role switch
            {
                "Admin" => "/?role=Admin",
                "Faculty" => "/?role=Faculty",
                _ => "/?role=Student",
            };
        }

        // Home page — render Index view
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [IgnoreAntiforgeryToken]
        public IActionResult Error()
        {
            var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(model);
        }
    }
}
