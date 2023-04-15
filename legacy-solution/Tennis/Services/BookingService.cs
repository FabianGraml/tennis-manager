using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis.DTOs;
using TennisDbLib;

namespace Tennis.Services
{
    public class BookingService
    {
        private TennisContext db;
        public BookingService(TennisContext db)
        {
            this.db = db;
        }
        public IEnumerable<Booking> GetBookings()
        {
            return db.Bookings.Include(x => x.Person).ToList();
        }
        public Booking GetSingleBooking(int id)
        {
            return db.Bookings.Include(x => x.Person).Single(x => x.Id == id);
        }
        public IEnumerable<Booking> GetBookingsByKW(int kw)
        {
            return db.Bookings.Include(x => x.Person).Where(x => x.Week == kw).ToList();
        }
        public Booking AddBooking(BookinPostDTO bookinPostDTO)
        {

            var booking = new Booking
            {
                DayOfWeek = bookinPostDTO.DayOfWeek,
                Hour = bookinPostDTO.Hour,
                Week = bookinPostDTO.Week,
                PersonId = bookinPostDTO.PersonId,
            };

            var checkbooking = db.Bookings.FirstOrDefault(x => x.DayOfWeek == bookinPostDTO.DayOfWeek && x.Hour == bookinPostDTO.Hour && x.Week == bookinPostDTO.Week);
            
            if (checkbooking == null)
            {
                
                db.Bookings.Add(booking);
                db.SaveChanges();
            }
            return booking;
        }
        public Booking EditBooking(int id,BookinPostDTO bookinPostDTO)
        {
            var booking = db.Bookings.Single(x => x.Id == id);
            booking.Hour = bookinPostDTO.Hour;
            booking.PersonId = bookinPostDTO.PersonId;
            booking.Week = bookinPostDTO.Week;
            booking.DayOfWeek = bookinPostDTO.DayOfWeek;
            db.SaveChanges();
            return booking;
        }
        
    }
}
