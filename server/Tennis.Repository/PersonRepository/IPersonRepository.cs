using Tennis.Database.Models;
using Tennis.Repository.GenericRepository;
namespace Tennis.Repository.PersonRepository;
public interface IPersonRepository : IGenericRepository<Person>
{
}