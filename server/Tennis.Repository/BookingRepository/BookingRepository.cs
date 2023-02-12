using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Tennis.Database.Context;
using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
namespace Tennis.Repository.BookingRepository;
public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(TennisContext dbContext) : base(dbContext) { }
    public async Task<IEnumerable<Booking>> GetAllIncludingAsync(Expression<Func<Booking, bool>> expression, Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> includes)
    {
        IQueryable<Booking> query = _dbContext.Set<Booking>();
        if(includes != null)
        {
            query = includes(query);
        }
        if(expression != null)
        {
            query = query.Where(expression);
        }
        return await query.ToListAsync();
    }
}