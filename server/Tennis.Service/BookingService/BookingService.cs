using Microsoft.EntityFrameworkCore;
using Tennis.Database.Models;
using Tennis.Model.DTOs;
using Tennis.Model.Models;
using Tennis.Model.Results;
using Tennis.Repository.UnitOfWork;
namespace Tennis.Service.BookingService;
public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    public BookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<ResponseModel, ResponseModel>> AddBooking(BookingDTO.BookingRequestDTO? bookingDTO)
    {
        if (bookingDTO == null)
        {
            return Result<ResponseModel, ResponseModel>.FromFailure(
                   new ResponseModel("Booking cannot be empty"), 400);
        }
        Booking? existingBooking = await _unitOfWork.BookingRepository
            .GetAsync(x => x.Week == bookingDTO.Week
            && x.DayOfWeek == bookingDTO.DayOfWeek
            && x.Hour == bookingDTO.Hour);
        if (existingBooking != null)
        {
            return Result<ResponseModel, ResponseModel>.FromFailure(
                   new ResponseModel("Cannot add Booking as slot is already occupied"), 400);
        }
        Booking? booking = new()
        {
            Week = bookingDTO?.Week == null ? -1 : bookingDTO.Week,
            DayOfWeek = bookingDTO?.DayOfWeek == null ? -1 : bookingDTO.DayOfWeek,
            Hour = bookingDTO?.Hour == null ? -1 : bookingDTO.Hour,
            UserId = bookingDTO?.UserId == null ? -1 : bookingDTO.UserId,
        };
        await _unitOfWork.BookingRepository.AddAsync(booking);
        await _unitOfWork.SaveAsync();
        return Result<ResponseModel, ResponseModel>.FromSuccess(
               new ResponseModel("Booking was added successfully"), 200);
    }
    public async Task<Result<ResponseModel, ResponseModel>> DeleteBooking(int id)
    {
        Booking? booking = await _unitOfWork.BookingRepository.GetAsync(x => x.Id == id);
        if (booking == null)
        {
            return Result<ResponseModel, ResponseModel>.FromFailure(
                   new ResponseModel($"Cannot find Booking with Id {id}"), 404);
        }
        _unitOfWork.BookingRepository.Remove(booking);
        await _unitOfWork.SaveAsync();
        return Result<ResponseModel, ResponseModel>.FromSuccess(
               new ResponseModel("Booking was deleted successfully"), 200);
    }
    public async Task<Result<IEnumerable<BookingDTO.BookingResponseDTO?>, ResponseModel>> GetAll()
    {
        IEnumerable<Booking>? bookings = await _unitOfWork.BookingRepository.GetAllIncludingAsync(null!, x => x.Include(y => y.User)!);
        if (bookings == null || !bookings.Any())
        {
            return Result<IEnumerable<BookingDTO.BookingResponseDTO?>, ResponseModel>.FromFailure(
                   new ResponseModel($"Cannot find any Bookings"), 404);
        }

        return Result<IEnumerable<BookingDTO.BookingResponseDTO>, ResponseModel>.FromSuccess(
              bookings.Select(x => new BookingDTO.BookingResponseDTO
              {
                  Id = x.Id,
                  Week = x.Week,
                  DayOfWeek = x.DayOfWeek,
                  Hour = x.Hour,
                  User = new UserDTO.UserResponseDTO()
                  {
                      Id = x.User?.Id ?? -1,
                      Firstname = x.User?.Firstname,
                      Lastname = x.User?.Lastname,
                      Email = x.User?.Email,
                  }
              }), 200)!;
    }
    public async Task<Result<IEnumerable<BookingDTO.BookingResponseDTO?>, ResponseModel>> GetBookingsForPerson(int userId)
    {
        IEnumerable<Booking>? bookings = await _unitOfWork.BookingRepository.GetAllIncludingAsync(x => x.UserId == userId, x => x.Include(y => y.User)!);
        if (bookings == null)
        {
            return Result<IEnumerable<BookingDTO.BookingResponseDTO?>, ResponseModel>.FromFailure(
                   new ResponseModel($"Cannot find any Bookings"), 404);
        }
        return Result<IEnumerable<BookingDTO.BookingResponseDTO>, ResponseModel>.FromSuccess(
              bookings.Select(x => new BookingDTO.BookingResponseDTO
              {
                  Id = x.Id,
                  Week = x.Week,
                  DayOfWeek = x.DayOfWeek,
                  Hour = x.Hour,
                  User = new UserDTO.UserResponseDTO()
                  {
                      Id = x.User?.Id ?? -1,
                      Firstname = x.User?.Firstname,
                      Lastname = x.User?.Lastname,
                      Email = x.User?.Email,
                  }
              }), 200)!;
    }
    public async Task<Result<BookingDTO.BookingResponseDTO?, ResponseModel>> GetById(int id)
    {
        Booking? booking = await _unitOfWork.BookingRepository.GetIncludingAsync(x => x.Id == id, x => x.Include(y => y.User)!);
        if (booking == null)
        {
            return Result<BookingDTO.BookingResponseDTO?, ResponseModel>.FromFailure(
                                        new ResponseModel($"Cannot find Booking with Id {id}"), 404);
        }
        return Result<BookingDTO.BookingResponseDTO, ResponseModel>.FromSuccess(
            new BookingDTO.BookingResponseDTO
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
            }, 200)!;
    }
    public async Task<Result<ResponseModel, ResponseModel>> UpdateBooking(int id, BookingDTO.BookingRequestDTO? bookingDTO)
    {
        Booking? booking = await _unitOfWork.BookingRepository.GetAsync(x => x.Id == id);
        if (booking == null)
        {
            return Result<ResponseModel, ResponseModel>.FromFailure(
                   new ResponseModel($"Cannot find Booking with Id {id}"), 404);
        }
        booking.Week = bookingDTO?.Week == null ? -1 : bookingDTO.Week;
        booking.DayOfWeek = bookingDTO?.DayOfWeek == null ? -1 : bookingDTO.DayOfWeek;
        booking.Hour = bookingDTO?.Hour == null ? -1 : bookingDTO.Hour;
        booking.UserId = bookingDTO?.UserId == null ? -1 : bookingDTO.UserId;
        _unitOfWork.BookingRepository.Update(booking);
        await _unitOfWork.SaveAsync();
        return Result<ResponseModel, ResponseModel>.FromSuccess(
               new ResponseModel("Booking was updated successfully"), 200);
    }
}