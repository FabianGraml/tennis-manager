using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tennis.Database.Context;
using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
namespace Tennis.Repository.BookingRepository;
public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(TennisContext dbContext) : base(dbContext) { }
    public async Task<IEnumerable<Booking>> GetAllIncludingAsync(Expression<Func<Booking, bool>> expression, Expression<Func<object, bool>>[] includes, CancellationToken cancellationToken = default)
    {
        IQueryable<Booking> query = _dbContext.Set<Booking>();
        foreach (var item in includes)
        {
            query = (IQueryable<Booking>)query.Include(item);
        }
        return await query.ToListAsync();
    }

    public async Task<Booking?> GetIncludingAsync(Expression<Func<Booking, bool>> expression, Expression<Func<Booking, object>>[] includes, CancellationToken cancellationToken = default)
    {
        IQueryable<Booking> query = _dbContext.Set<Booking>();
        foreach (var item in includes)
        {
            query = query.Include(item);
        }
        return await query.FirstOrDefaultAsync(expression);
    }
}