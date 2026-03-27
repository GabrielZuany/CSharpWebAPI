using Domain.Enums;
using Domain.BaseDomain;

namespace Domain.Users.Model;

public class User : Base
{
    public string Username { get; set; }
    public string Password { get; set; }
    public LevelAccessEnum LevelAccess { get; set; }
}