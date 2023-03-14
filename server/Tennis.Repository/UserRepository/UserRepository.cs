using Tennis.Database.Context;
using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
namespace Tennis.Repository.UserRepository;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(TennisContext dbContext) : base(dbContext)
    {
    }
}
