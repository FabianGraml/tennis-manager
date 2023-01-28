using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tennis.Model.DTOs;
using Tennis.Service.PersonService;
namespace Tennis.Controllers;
[ApiController]
[Route("api/person")]
public class PersonsController : Controller
{
    private readonly IPersonService _personService;
    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPersons()
    {
        return Ok(await _personService.GetAll());
    }
    [HttpGet("single")]
    public async Task<IActionResult> GetPersonByID([FromQuery] int personId)
    {
        return Ok(await _personService.GetById(personId));
    }
    [HttpDelete("remove")]
    public async Task<IActionResult> RemovePerson([FromQuery] int personId)
    {
        await _personService.DeletePerson(personId);
        return NoContent();
    }
    [HttpPost("add")]
    public async Task<IActionResult> AddPerson([FromBody] PersonDTO.PersonRequestDTO personDTO)
    {
        await _personService.AddPerson(personDTO);
        return Ok();
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdatePerson([FromQuery] int personId, [FromBody] PersonDTO.PersonRequestDTO personDTO)
    {
        await _personService.UpdatePerson(personId, personDTO);
        return Ok();
    }
}