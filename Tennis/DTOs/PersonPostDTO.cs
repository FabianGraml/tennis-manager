using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis.DTOs
{
    public class PersonPostDTO
    {
        [MinLength(3)]
        public string Firstname { get; set; }
        [MinLength(3)]
        public string Lastname { get; set; }
        [Range(10, 90)]
        public int Age { get; set; }
    }
}
