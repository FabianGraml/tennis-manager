using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tennis.Database.Models;
using Tennis.Model.DTOs;
using Tennis.Repository.UnitOfWork;
namespace Tennis.Service.BookingService;
public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    public BookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task AddBooking(BookingDTO.BookingRequestDTO? bookingDTO)
    {
        Booking? booking = new Booking
        {
            Week = bookingDTO?.Week == null ? -1 : bookingDTO.Week,
            DayOfWeek = bookingDTO?.DayOfWeek == null ? -1 : bookingDTO.DayOfWeek,
            Hour = bookingDTO?.Hour == null ? -1 : bookingDTO.Hour,
            PersonId = bookingDTO?.PersonId == null ? -1 : bookingDTO.PersonId,
        };
        await _unitOfWork.BookingRepository.AddAsync(booking);
        await _unitOfWork.SaveAsync();
    }
    public async Task DeleteBooking(int id)
    {
        Booking? booking = await _unitOfWork.BookingRepository.GetAsync(x => x.Id == id);
        if (booking == null)
        {
            throw new ArgumentException($"Couldn't find booking with ID {id}");
        }
        _unitOfWork.BookingRepository.Remove(booking);
        await _unitOfWork.SaveAsync();
    }
    public async Task<IEnumerable<BookingDTO.BookingResponseDTO?>> GetAll()
    {
        IEnumerable<Booking>? bookings = await _unitOfWork.BookingRepository.GetAllAsync();
        return bookings.Select(x => new BookingDTO.BookingResponseDTO
        {
            Id = x.Id,
            Week = x.Week,
            DayOfWeek = x.DayOfWeek,
            Hour = x.Hour,
            Person = new PersonDTO.PersonResponseDTO
            {
                Id = x.Person?.Id == null ? -1 : x.Person.Id,
                Age = x.Person?.Age == null ? -1 : x.Person.Age,
                Firstname = x.Person?.Firstname,
                Lastname = x.Person?.Lastname,
            }
        });
    }
    public async Task<IEnumerable<BookingDTO.BookingResponseDTO?>> GetBookingsForPerson(int personId)
    {
        IEnumerable<Booking>? bookings = await _unitOfWork.BookingRepository.GetAllIncludingAsync(x => x.PersonId == personId, x => x.Include(y => y.Person)!);
        return bookings.Select(x => new BookingDTO.BookingResponseDTO
        {
            Id = x.Id,
            Week = x.Week,
            DayOfWeek = x.DayOfWeek,
            Hour = x.Hour,
            Person = new PersonDTO.PersonResponseDTO
            {
                Id = x.Person?.Id == null ? -1 : x.Person.Id,
                Age = x.Person?.Age == null ? -1 : x.Person.Age,
                Firstname = x.Person?.Firstname,
                Lastname = x.Person?.Lastname,
            }
        });
    }
    public async Task<BookingDTO.BookingResponseDTO?> GetById(int id)
    {
        Booking? booking = await _unitOfWork.BookingRepository.GetAsync(x => x.Id == id);
        if (booking == null)
        {
            throw new ArgumentException($"Couldn't find booking with ID {id}");
        }
        return new BookingDTO.BookingResponseDTO
        {
            Id = booking.Id,
            DayOfWeek = booking.DayOfWeek,
            Hour = booking.Hour,
            Week = booking.Week,
            Person = new PersonDTO.PersonResponseDTO
            {
                Id = booking.Person?.Id == null ? -1 : booking.Person.Id,
                Age = booking.Person?.Age == null ? -1 : booking.Person.Age,
                Firstname = booking.Person?.Firstname,
                Lastname = booking.Person?.Lastname,
            }
        };
    }
    public async Task UpdateBooking(int id, BookingDTO.BookingRequestDTO? bookingDTO)
    {
        Booking? booking = await _unitOfWork.BookingRepository.GetAsync(x => x.Id == id);
        if (booking == null)
        {
            throw new ArgumentException($"Booking with Id {id} could not be found");
        }
        booking.Week = bookingDTO?.Week == null ? -1 : booking.Week;
        booking.DayOfWeek = bookingDTO?.DayOfWeek == null ? -1 : booking.DayOfWeek;
        booking.Hour = bookingDTO?.Hour == null ? -1 : booking.Hour;
        booking.PersonId = bookingDTO?.PersonId == null ? -1 : bookingDTO.PersonId;
        _unitOfWork.BookingRepository.Update(booking);
        await _unitOfWork.SaveAsync();
    }
}