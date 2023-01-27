using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis.DTOs
{
    public class BookinPostDTO
    {
        [Range(1, 52)]
        public int Week { get; set; }
        [Range(1, 7)]
        public int DayOfWeek { get; set; }
        [Range(6, 22)] //Tennis court only open from 06:00 to 22:00
        public int Hour { get; set; }
        public int PersonId { get; set; }
    }
}
