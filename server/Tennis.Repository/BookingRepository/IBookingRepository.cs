using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
namespace Tennis.Repository.BookingRepository;
public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetAllIncludingAsync(Expression<Func<Booking, bool>> expression, Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> includes);
}