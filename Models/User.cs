namespace Book2Enter.Models
{
    public class User
    {
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // plaintext for demo only
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "Student"; // Admin/Faculty/Student
        public bool IsFirstLogin { get; set; } = true;

        // OTP
        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }
    }
}