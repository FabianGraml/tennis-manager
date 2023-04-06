using Microsoft.AspNetCore.Mvc;
using Tennis.Api.Controllers;
using Tennis.Model.DTOs;
using Tennis.Service.BookingService;
namespace Tennis.Controllers;
[Route("api/booking")]
[ApiController]
public class BookingsController : BaseController
{
    private readonly IBookingService _bookingService;
    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return await ExecuteAsync(_bookingService.GetAll());
    }
    [HttpGet("person")]
    public async Task<IActionResult> GetBookingsByPerson([FromQuery] int personId)
    {
        return await ExecuteAsync(_bookingService.GetBookingsForPerson(personId));
    }
    [HttpGet("single")]
    public async Task<IActionResult> GetBookingById([FromQuery] int bookingId)
    {
        return await ExecuteAsync(_bookingService.GetById(bookingId));
    }
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveBooking([FromQuery] int bookingId)
    {
        return await ExecuteAsync(_bookingService.DeleteBooking(bookingId));
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddBooking([FromBody] BookingDTO.BookingRequestDTO? bookingDTO)
    {
        return await ExecuteAsync(_bookingService.AddBooking(bookingDTO));
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateBooking([FromQuery] int bookingId, [FromBody] BookingDTO.BookingRequestDTO bookingDTO)
    {
        return await ExecuteAsync(_bookingService.UpdateBooking(bookingId, bookingDTO));
    }
}