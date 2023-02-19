namespace Tennis.Database.Models;
public class UserRole
{
    public int Id { get; set; }
    public string? RoleName { get; set; }
    public virtual IEnumerable<User>? Users { get; set; }
}
