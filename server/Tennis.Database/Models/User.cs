namespace Tennis.Database.Models;
public class User
{
    public int Id { get; set; }
    public string? Firstname { get; set; } = string.Empty;
    public string? Lastname { get; set;} = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public virtual UserRole? Role { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set;}
    public bool IsActivated { get; set; }
    public DateTime CreatedAt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
}
