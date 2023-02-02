using Tennis.Model.DTOs;
namespace Tennis.Service.PersonService;
public interface IPersonService
{
    Task<IEnumerable<PersonDTO.PersonResponseDTO?>> GetAll();
    Task<PersonDTO.PersonResponseDTO?> GetById(int id);
    Task AddPerson(PersonDTO.PersonRequestDTO? personDTO);
    Task UpdatePerson(int id, PersonDTO.PersonRequestDTO? personDTO);
    Task DeletePerson(int id);
}