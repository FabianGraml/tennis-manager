using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tennis.Model.DTOs;
using Tennis.Service.BookingService;
namespace Tennis.Controllers;
[Authorize]
[Route("api/booking")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _bookingService.GetAll());
    }
    [HttpGet("person")]
    public async Task<IActionResult> GetBookingsByPerson([FromQuery] int personId)
    {
        return Ok(await _bookingService.GetBookingsForPerson(personId));
    }
    [HttpGet("single")]
    public async Task<IActionResult> GetBookingById([FromQuery] int bookingId)
    {
        return Ok(await _bookingService.GetById(bookingId));
    }
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveBooking([FromQuery] int bookingId)
    {
        await _bookingService.DeleteBooking(bookingId);
        return NoContent();
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddBooking([FromBody] BookingDTO.BookingRequestDTO? bookingDTO)
    {
        await _bookingService.AddBooking(bookingDTO);
        return Ok();
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateBooking([FromQuery] int bookingId, [FromBody] BookingDTO.BookingRequestDTO bookingDTO)
    {
        await _bookingService.UpdateBooking(bookingId, bookingDTO);
        return Ok();
    }
}