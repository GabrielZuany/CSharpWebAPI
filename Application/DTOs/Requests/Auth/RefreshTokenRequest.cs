namespace Application.DTOs.Requests.Auth;

public record RefreshTokenRequest
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
}