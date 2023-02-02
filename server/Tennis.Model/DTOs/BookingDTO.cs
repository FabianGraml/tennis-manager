namespace Tennis.Model.DTOs;
public class BookingDTO
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public int Week { get; set; }
        public int DayOfWeek { get; set; }
        public int Hour { get; set; }
        public PersonDTO.PersonResponseDTO? Person { get; set; }
    }
    public class BookingRequestDTO
    {
        public int Week { get; set; }
        public int DayOfWeek { get; set; }
        public int Hour { get; set; }
        public int PersonId { get; set; }
    }
}