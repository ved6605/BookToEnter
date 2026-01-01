using System;
using System.Collections.Generic;

namespace Book2Enter.Models
{
    public class DashboardViewModel
    {
        public string StudentName { get; set; } = "";
        public int UpcomingEventsCount { get; set; }
        public int BookedEventsCount { get; set; }
        public int ActiveEntriesCount { get; set; }
        public int PendingActionsCount { get; set; }

        public EventViewModel? NextEvent { get; set; }
        public List<BookingViewModel> Bookings { get; set; } = new();
        public List<EventBriefViewModel> AvailableEvents { get; set; } = new();
        public List<string> Notifications { get; set; } = new();
    }

    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime StartUtc { get; set; }
        public string Venue { get; set; } = "";
        public int Capacity { get; set; }
    }

    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public string EventName { get; set; } = "";
        public DateTime EventStartUtc { get; set; }
        public string Status { get; set; } = ""; // Booked, Attended, Missed
        public string QrStatus { get; set; } = ""; // Locked, Active, Used
    }

    public class EventBriefViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Date { get; set; }
    }
}