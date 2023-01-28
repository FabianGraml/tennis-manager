namespace Tennis.Database.Models;
public class Booking
{
    public int Id { get; set; }
    public int Week { get; set; }
    public int DayOfWeek { get; set; }
    public int Hour { get; set; }
    public int PersonId { get; set; }
    public virtual Person Person { get; set; }
}