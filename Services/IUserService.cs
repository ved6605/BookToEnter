using Book2Enter.Models;

namespace Book2Enter.Services
{
    public interface IUserService
    {
        User? GetByUserId(string userId);
        (bool ok, string message) ValidateCredentials(string userId, string password);
        string IdentifyRoleFromUserId(string userId);
        string GenerateOtp(string userId);
        bool VerifyOtp(string userId, string otp);
        void SetPassword(string userId, string newPassword);
    }
}