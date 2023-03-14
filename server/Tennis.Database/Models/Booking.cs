namespace Tennis.Database.Models;
public class Booking
{
    public int Id { get; set; }
    public int Week { get; set; }
    public int DayOfWeek { get; set; }
    public int Hour { get; set; }
    public int UserId { get; set; }
    public virtual User? User { get; set; }
}