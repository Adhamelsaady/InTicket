using InTicket.Application.Contracts;

namespace InTicket.Application.Services;

public class OtpService : IOtpService
{
    public string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    public bool ValidateOtp(string providedOtp, string storedOtp, DateTime? expiresAt)
    {
        if (string.IsNullOrEmpty(providedOtp) || string.IsNullOrEmpty(storedOtp))
            return false;

        if (expiresAt == null || DateTime.UtcNow > expiresAt)
            return false;

        return providedOtp == storedOtp;
    }
}