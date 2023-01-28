using Tennis.Database.Context;
using Tennis.Repository.BookingRepository;
using Tennis.Repository.PersonRepository;
namespace Tennis.Repository.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly TennisContext _dbContext;
    public IBookingRepository BookingRepository { get; set; }
    public IPersonRepository PersonRepository { get; set; }

    public UnitOfWork(TennisContext dbContext)
    {
        _dbContext = dbContext;
        BookingRepository = new BookingRepository.BookingRepository(dbContext);
        PersonRepository = new PersonRepository.PersonRepository(dbContext);
    }
    public async Task RollbackAsync()
        => await _dbContext.DisposeAsync();
    public async Task SaveAsync()
        => await _dbContext.SaveChangesAsync();
}
