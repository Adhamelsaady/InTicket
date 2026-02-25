namespace InTicket.Domain.Dtos;

public class AuthResult
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public bool Success { get; set; }
    public ICollection<string> Errors { get; set; } = new List<string>();
}