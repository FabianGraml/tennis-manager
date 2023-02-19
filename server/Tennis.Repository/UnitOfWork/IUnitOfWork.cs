using Tennis.Repository.BookingRepository;
using Tennis.Repository.PersonRepository;
using Tennis.Repository.UserRepository;

namespace Tennis.Repository.UnitOfWork;
public interface IUnitOfWork
{
    IBookingRepository BookingRepository { get; set; }
    IPersonRepository PersonRepository { get; set; }
    IUserRepository UserRepository { get; set; }
    Task SaveAsync();
    Task RollbackAsync();
}