using System.Collections.Generic;
namespace Tennis.Database.Models;
public class Person
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public int Age { get; set; }
    public virtual IEnumerable<Booking> Bookings { get; set; }
}