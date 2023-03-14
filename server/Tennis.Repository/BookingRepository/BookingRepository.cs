using Tennis.Database.Context;
using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
namespace Tennis.Repository.BookingRepository;
public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(TennisContext dbContext) : base(dbContext) { }
}