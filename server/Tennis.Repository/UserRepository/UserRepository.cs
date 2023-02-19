using Tennis.Database.Context;
using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
using Tennis.Repository.PersonRepository;
namespace Tennis.Repository.UserRepository;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(TennisContext dbContext) : base(dbContext)
    {
    }
}
