using Tennis.Model.DTOs;
using Tennis.Model.Models;
using Tennis.Model.Results;

namespace Tennis.Service.BookingService;
public interface IBookingService
{
    Task<Result<IEnumerable<BookingDTO.BookingResponseDTO?>, ResponseModel>> GetAll();
    Task<Result<BookingDTO.BookingResponseDTO?, ResponseModel>> GetById(int id);
    Task<Result<ResponseModel, ResponseModel>> AddBooking(BookingDTO.BookingRequestDTO? bookingDTO);
    Task<Result<ResponseModel, ResponseModel>> UpdateBooking(int id, BookingDTO.BookingRequestDTO? bookingDTO);
    Task<Result<ResponseModel, ResponseModel>> DeleteBooking(int id);
    Task<Result<IEnumerable<BookingDTO.BookingResponseDTO?>, ResponseModel>> GetBookingsForPerson(int personId);
}