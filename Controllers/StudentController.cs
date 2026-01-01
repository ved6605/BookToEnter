using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Book2Enter.Models;

namespace Book2Enter.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Dashboard()
        {
            var vm = new DashboardViewModel
            {
                StudentName = User?.Identity?.Name ?? "Student",
                UpcomingEventsCount = 1,
                BookedEventsCount = 2,
                ActiveEntriesCount = 0,
                PendingActionsCount = 0,
                NextEvent = new EventViewModel { Id = 42, Name = "TechFest", StartUtc = DateTime.UtcNow.AddHours(5), Venue = "Auditorium" },
                Bookings = new List<BookingViewModel>
                {
                    new BookingViewModel { BookingId = 1, EventName = "TechFest", EventStartUtc = DateTime.UtcNow.AddHours(5), Status = "Booked", QrStatus = "Locked" },
                    new BookingViewModel { BookingId = 2, EventName = "Cultural Night", EventStartUtc = DateTime.UtcNow.AddDays(3), Status = "Booked", QrStatus = "Locked" }
                },
                AvailableEvents = new List<EventBriefViewModel>
                {
                    new EventBriefViewModel { Id = 55, Name = "Workshop X", Date = DateTime.UtcNow.AddDays(6) }
                },
                Notifications = new List<string> { "Your entry pass will activate 30 minutes before the event." }
            };

            return View(vm);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult BookingQr(int id)
        {
            // For now return a simple SVG placeholder QR image.
            var svg = $"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 300 300' width='300' height='300'>" +
                      $"<rect width='100%' height='100%' fill='#0B1220' />" +
                      $"<text x='50%' y='50%' fill='#00E0B8' font-size='28' text-anchor='middle' dominant-baseline='middle'>QR #{id}</text>" +
                      "</svg>";
            return File(System.Text.Encoding.UTF8.GetBytes(svg), "image/svg+xml");
        }
    }
}