using Tennis.Database.Context;
using Tennis.Repository.BookingRepository;
using Tennis.Repository.UserRepository;

namespace Tennis.Repository.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly TennisContext _dbContext;
    public IBookingRepository BookingRepository { get; set; }
    public IUserRepository UserRepository { get; set; }

    public UnitOfWork(TennisContext dbContext)
    {
        _dbContext = dbContext;
        BookingRepository = new BookingRepository.BookingRepository(dbContext);
        UserRepository = new UserRepository.UserRepository(dbContext);
    }
    public async Task RollbackAsync()
        => await _dbContext.DisposeAsync();
    public async Task SaveAsync()
        => await _dbContext.SaveChangesAsync();
}
