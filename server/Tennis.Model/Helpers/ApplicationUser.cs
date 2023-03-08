namespace Tennis.Model.Helpers;
public class ApplicationUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
