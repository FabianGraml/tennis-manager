using Tennis.Database.Models;
using Tennis.Model.DTOs;
using Tennis.Repository.UnitOfWork;
namespace Tennis.Service.PersonService;
public class PersonService : IPersonService
{
    private readonly IUnitOfWork _unitOfWork;
    public PersonService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task AddPerson(PersonDTO.PersonRequestDTO? personDTO)
    {
        Person? person = new Person
        {
            Firstname = personDTO?.Firstname,
            Lastname = personDTO?.Lastname,
            Age = personDTO == null ? -1 : personDTO.Age,
        };
        await _unitOfWork.PersonRepository.AddAsync(person);
        await _unitOfWork.SaveAsync();
    }
    public async Task DeletePerson(int id)
    {
        Person? person = await _unitOfWork.PersonRepository.GetAsync(x => x.Id == id);
        if (person == null)
        {
            throw new ArgumentException($"Person with Id {id} could not be found");
        }
        _unitOfWork.PersonRepository.Remove(person);
        await _unitOfWork.SaveAsync();
    }
    public async Task<IEnumerable<PersonDTO.PersonResponseDTO?>> GetAll()
    {
        IEnumerable<Person> persons = 
            await _unitOfWork.PersonRepository.GetAllAsync();
        return persons.Select(x => new PersonDTO.PersonResponseDTO
        {
            Id = x.Id,
            Firstname = x.Firstname,
            Lastname = x.Lastname,
            Age = x.Age,
        }).ToList();
    }
    public async Task<PersonDTO.PersonResponseDTO?> GetById(int id)
    {
        Person? person = await _unitOfWork.PersonRepository.GetAsync(x => x.Id == id);
        if (person == null)
        {
            throw new ArgumentException($"Person with Id {id} could not be found");
        }
        return new PersonDTO.PersonResponseDTO
        {
            Id = person.Id,
            Firstname = person.Firstname,
            Lastname = person.Lastname,
            Age = person.Age,
        };
    }
    public async Task UpdatePerson(int id, PersonDTO.PersonRequestDTO? personDTO)
    {
        Person? person = await _unitOfWork.PersonRepository.GetAsync(x => x.Id == id);
        if (person == null)
        {
            throw new ArgumentException($"Person with Id {id} could not be found");
        }
        person.Firstname = personDTO?.Firstname == null ? person.Firstname : personDTO.Firstname;
        person.Lastname = personDTO?.Lastname == null ? person.Lastname : personDTO.Lastname;   
        person.Age = personDTO?.Age == null ? person.Age : personDTO.Age;
        _unitOfWork.PersonRepository.Update(person);
        await _unitOfWork.SaveAsync();
    }
}