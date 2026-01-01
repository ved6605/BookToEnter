using Book2Enter.Models;

namespace Book2Enter.Services
{
    public class UserService : IUserService
    {
        private readonly Dictionary<string, User> _users = new();
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;

            // Seed some users (Admin ID is a static predefined ID: "admin")
            _users["admin"] = new User
            {
                UserId = "admin",
                Password = "adminpass",
                Email = "admin@utu.ac.in",
                Role = "Admin",
                IsFirstLogin = false
            };

            _users["S12345"] = new User
            {
                UserId = "S12345",
                Password = "studpass",
                Email = "student1@utu.ac.in",
                Role = "Student",
                IsFirstLogin = true
            };

            _users["prof@utu.ac.in"] = new User
            {
                UserId = "prof@utu.ac.in",
                Password = "facpass",
                Email = "prof@utu.ac.in",
                Role = "Faculty",
                IsFirstLogin = true
            };
        }

        public User? GetByUserId(string userId)
        {
            _users.TryGetValue(userId, out var user);
            return user;
        }

        public (bool ok, string message) ValidateCredentials(string userId, string password)
        {
            var user = GetByUserId(userId);
            if (user == null)
            {
                return (false, "User not found");
            }

            if (user.Password != password)
            {
                return (false, "Invalid password");
            }

            return (true, "OK");
        }

        // Simple heuristics to identify role based on user id
        public string IdentifyRoleFromUserId(string userId)
        {
            if (string.Equals(userId, "admin", StringComparison.OrdinalIgnoreCase)) return "Admin";
            if (userId.Contains("@")) return "Faculty";
            // Enrollment number heuristic: starts with letter or digit and no @
            return "Student";
        }

        public string GenerateOtp(string userId)
        {
            var user = GetByUserId(userId);
            if (user == null) throw new ArgumentException("Unknown user");
            var rng = new Random();
            var otp = rng.Next(100000, 999999).ToString();
            user.Otp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

            // In a real app: send OTP to user's email. Here we log it and pretend to send.
            _logger.LogInformation("[OTP] Sent OTP {Otp} to {Email} for user {UserId}", otp, user.Email, userId);

            return otp;
        }

        public bool VerifyOtp(string userId, string otp)
        {
            var user = GetByUserId(userId);
            if (user == null || user.Otp == null || user.OtpExpiry == null) return false;
            if (DateTime.UtcNow > user.OtpExpiry) return false;
            return string.Equals(user.Otp, otp, StringComparison.Ordinal);
        }

        public void SetPassword(string userId, string newPassword)
        {
            var user = GetByUserId(userId);
            if (user == null) throw new ArgumentException("Unknown user");
            user.Password = newPassword;
            user.IsFirstLogin = false;
            // clear OTP
            user.Otp = null;
            user.OtpExpiry = null;
        }
    }
}