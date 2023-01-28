using Tennis.Repository.BookingRepository;
using Tennis.Repository.PersonRepository;
namespace Tennis.Repository.UnitOfWork;
public interface IUnitOfWork
{
    IBookingRepository BookingRepository { get; set; }
    IPersonRepository PersonRepository { get; set; }
    Task SaveAsync();
    Task RollbackAsync();
}