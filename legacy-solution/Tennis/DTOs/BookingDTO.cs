using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int Week { get; set; }
        public int DayOfWeek { get; set; }
        public int Hour { get; set; }
        public int PersonId { get; set; }
        public PersonDTO PersonDTO { get; set; }
    }
}
