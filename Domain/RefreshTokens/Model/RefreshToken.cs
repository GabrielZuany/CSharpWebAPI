using Domain.BaseDomain;
using Domain.Users.Model;

namespace Domain.RefreshTokens.Model;

public class RefreshToken : Base
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public User User { get; set; } 
}