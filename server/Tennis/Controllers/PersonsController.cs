using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis.DTOs;
using Tennis.Extensionmethods;
using Tennis.Services;

namespace Tennis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private PersonService personService;

        public PersonsController(PersonService personService)
        {
            this.personService = personService;
        }

        [HttpGet]
        public IEnumerable<PersonDTO> GetPersons() {
            return personService.GetPersons().Select(x => new PersonDTO().CopyPropertiesFrom(x));
        }
        [HttpGet("{id}")]
        public PersonDTO GetSinglePerson(int id)
        {
            return new PersonDTO().CopyPropertiesFrom(personService.GetSinglePerson(id));
        }
        [HttpPost]
        public PersonPostDTO AddPerson(PersonPostDTO personPostDTO)
        {
            return new PersonPostDTO().CopyPropertiesFrom(personService.AddPerson(personPostDTO));
        }
        [HttpPut("{id}")]
        public PersonPostDTO EditPerson(int id, PersonPostDTO personPostDTO)
        {
            return new PersonPostDTO().CopyPropertiesFrom(personService.EditPerson(id, personPostDTO));
        }
        [HttpDelete("{id}")]
        public PersonDTO DeletePerson(int id)
        {
            return new PersonDTO().CopyPropertiesFrom(personService.DeletePerson(id));
        }

    }
}
