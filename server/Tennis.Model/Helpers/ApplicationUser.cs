using Microsoft.AspNet.Identity.EntityFramework;
namespace Tennis.Model.Helpers;
public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
