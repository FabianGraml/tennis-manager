namespace Tennis.Model.DTOs;
public class UserDTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
    }
}