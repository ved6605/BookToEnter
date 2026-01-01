using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book2Enter.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class CoordinatorController : Controller
    {
        // GET: /Coordinator/Dashboard
        [HttpGet]
        [AllowAnonymous] // keep anonymous for dev/testing; secure this in production
        public IActionResult Dashboard()
        {
            return View();
        }

        // POST: /Coordinator/ValidateQr
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ValidateQr([FromForm] string qrData)
        {
            // Demo behaviour: parse qrData (in real app validate token and fetch booking details)
            if (string.IsNullOrWhiteSpace(qrData))
                return Json(new { success = false, error = "Empty QR data" });

            // Return demo student details
            var student = new {
                name = "Ved Patel",
                enrollment = "ENR2025001",
                email = "ved.patel@university.edu",
                idImageUrl = Url.Content("~/images/id-placeholder.svg")
            };

            return Json(new { success = true, student });
        }
    }
}