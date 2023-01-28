using Tennis.Database.Context;
using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
namespace Tennis.Repository.PersonRepository;
public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    public PersonRepository(TennisContext dbContext) : base(dbContext)
    {
    }
}