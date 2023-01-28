namespace Tennis.Model.DTOs;
public class PersonDTO
{
    public class PersonResponseDTO
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int Age { get; set; }
    }
    public class PersonRequestDTO
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int Age { get; set; }
    }
}