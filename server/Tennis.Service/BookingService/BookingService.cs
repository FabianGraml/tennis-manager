using Microsoft.EntityFrameworkCore;
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
        Booking? booking = new()
        {
            Week = bookingDTO?.Week == null ? -1 : bookingDTO.Week,
            DayOfWeek = bookingDTO?.DayOfWeek == null ? -1 : bookingDTO.DayOfWeek,
            Hour = bookingDTO?.Hour == null ? -1 : bookingDTO.Hour,
            UserId = bookingDTO?.UserId == null ? -1 : bookingDTO.UserId,
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
            User = new UserDTO.UserResponseDTO()
            {
                Id = x.User?.Id == null ? -1 : x.User.Id,
                Firstname = x.User?.Firstname,
                Lastname = x.User?.Lastname,
                Email = x.User?.Email,
            }
        });
    }
    public async Task<IEnumerable<BookingDTO.BookingResponseDTO?>> GetBookingsForPerson(int userId)
    {
        IEnumerable<Booking>? bookings = await _unitOfWork.BookingRepository.GetAllIncludingAsync(x => x.UserId == userId, x => x.Include(y => y.User)!);
        if (bookings == null)
        {
            throw new ApplicationException("Cannot find any bookings");
        }
        return bookings.Select(x => new BookingDTO.BookingResponseDTO
        {
            Id = x.Id,
            Week = x.Week,
            DayOfWeek = x.DayOfWeek,
            Hour = x.Hour,
            User = new UserDTO.UserResponseDTO
            {
                Id = x.User?.Id == null ? -1 : x.User.Id,
                Firstname = x.User?.Firstname,
                Lastname = x.User?.Lastname,
                Email = x.User?.Email,
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
            User = new UserDTO.UserResponseDTO
            {
                Id = booking.User?.Id == null ? -1 : booking.User.Id,
                Firstname = booking.User?.Firstname,
                Lastname = booking.User?.Lastname,
                Email = booking.User?.Email,
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
        booking.UserId = bookingDTO?.UserId == null ? -1 : bookingDTO.UserId;
        _unitOfWork.BookingRepository.Update(booking);
        await _unitOfWork.SaveAsync();
    }
}