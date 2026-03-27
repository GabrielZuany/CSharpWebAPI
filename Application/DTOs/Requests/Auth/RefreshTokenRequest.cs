namespace Application.DTOs.Requests.Auth;

public record RefreshTokenRequest
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
}