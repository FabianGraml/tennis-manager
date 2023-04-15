using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis.DTOs;
using Tennis.Extensionmethods;
using Tennis.Services;

namespace Tennis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingsController : ControllerBase
    {
        private BookingService bookingService;

        public BookingsController(BookingService bookingService)
        {
            this.bookingService = bookingService;
        }
        [HttpGet]
        public IEnumerable<BookingDTO> GetBookings()
        {
            return bookingService.GetBookings().Select(x => new BookingDTO
            {
                Id = x.Id,
                DayOfWeek = x.DayOfWeek,
                Hour = x.Hour,
                PersonId = x.PersonId,
                Week = x.Week,
                PersonDTO = new PersonDTO().CopyPropertiesFrom(x.Person)
            }
            );
        }
        [HttpGet("{id}")]
        public BookingDTO GetSingleBooking(int id)
        {
            var booking = bookingService.GetSingleBooking(id);
            return new BookingDTO
            {
                Id = booking.Id,
                DayOfWeek = booking.DayOfWeek,
                Hour = booking.Hour,
                PersonDTO = new PersonDTO().CopyPropertiesFrom(booking.Person),
                Week = booking.Week,
                PersonId = booking.PersonId,
            };

        }
        [HttpGet("calendarWeek/{kw}")]
        public IEnumerable<BookingDTO> GetBookingsKW(int kw)
        {
            return bookingService.GetBookingsByKW(kw).Select(x => new BookingDTO
            {
                Id = x.Id,
                DayOfWeek = x.DayOfWeek,
                Hour = x.Hour,
                PersonId = x.PersonId,
                Week = x.Week,
                PersonDTO = new PersonDTO().CopyPropertiesFrom(x.Person)
            }
           );
        }
        [HttpPost]
        public BookinPostDTO AddBooking(BookinPostDTO bookinPostDTO)
        {
            Console.WriteLine("BookingsController::AddBooking");
            return new BookinPostDTO().CopyPropertiesFrom(bookingService.AddBooking(bookinPostDTO));
        }
        [HttpPut("{id}")]
        public BookinPostDTO EditBooking(int id, BookinPostDTO bookinPostDTO)
        {
            Console.WriteLine("BookingsController::EditBookings");
            return new BookinPostDTO().CopyPropertiesFrom(bookingService.EditBooking(id, bookinPostDTO));
        }
        [HttpGet("date")]
        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
