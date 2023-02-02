using System.Linq.Expressions;
using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
namespace Tennis.Repository.BookingRepository;
public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetAllIncludingAsync(Expression<Func<Booking, bool>> expression, Expression<Func<object, bool>>[] includes, CancellationToken cancellationToken = default);
    Task<Booking?> GetIncludingAsync(Expression<Func<Booking, bool>> expression, Expression<Func<Booking, object>>[] includes, CancellationToken cancellationToken = default);
}