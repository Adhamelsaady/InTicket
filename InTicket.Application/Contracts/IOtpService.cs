namespace InTicket.Application.Contracts;

public interface IOtpService
{
    string GenerateOtp();
    bool ValidateOtp(string providedOtp, string storedOtp, DateTime? expiresAt);
}