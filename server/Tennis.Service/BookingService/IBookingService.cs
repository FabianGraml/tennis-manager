using Tennis.Model.DTOs;
namespace Tennis.Service.BookingService;
public interface IBookingService
{
    Task<IEnumerable<BookingDTO.BookingResponseDTO?>> GetAll();
    Task<BookingDTO.BookingResponseDTO?> GetById(int id);
    Task AddBooking(BookingDTO.BookingRequestDTO? bookingDTO);
    Task UpdateBooking(int id, BookingDTO.BookingRequestDTO? bookingDTO);
    Task DeleteBooking(int id);
    Task<IEnumerable<BookingDTO.BookingResponseDTO?>> GetBookingsForPerson(int personId);
}