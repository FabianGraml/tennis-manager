using Tennis.Repository.BookingRepository;
using Tennis.Repository.UserRepository;

namespace Tennis.Repository.UnitOfWork;
public interface IUnitOfWork
{
    IBookingRepository BookingRepository { get; set; }
    IUserRepository UserRepository { get; set; }
    Task SaveAsync();
    Task RollbackAsync();
}