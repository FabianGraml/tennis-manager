namespace Tennis.Model.DTOs;
public class BookingDTO
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public int Week { get; set; }
        public int DayOfWeek { get; set; }
        public int Hour { get; set; }
        public UserDTO.UserResponseDTO? User { get; set; }
    }
    public class BookingRequestDTO
    {
        public int Week { get; set; }
        public int DayOfWeek { get; set; }
        public int Hour { get; set; }
        public int UserId { get; set; }
    }
}