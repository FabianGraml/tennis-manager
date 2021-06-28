using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis.DTOs;
using TennisDbLib;

namespace Tennis.Services
{
    public class PersonService
    {
        private TennisContext db;
        public PersonService(TennisContext db)
        {
            this.db = db;
        }
        public IEnumerable<Person> GetPersons()
        {
            return db.Persons.ToList();
        }
        public Person GetSinglePerson(int id)
        {
            return db.Persons.Single(x => x.Id == id);
        }
        public Person AddPerson(PersonPostDTO personPostDTO)
        {
            var person = new Person
            {
                Age = personPostDTO.Age,
                Firstname = personPostDTO.Firstname,
                Lastname = personPostDTO.Lastname,
            };
            db.Persons.Add(person);
            db.SaveChanges();
            return person;
        }
        public Person EditPerson(int id, PersonPostDTO personPostDTO)
        {
            var person = db.Persons.Single(x => x.Id == id);
            person.Firstname = personPostDTO.Firstname;
            person.Lastname = personPostDTO.Lastname;
            person.Age = personPostDTO.Age;
            db.SaveChanges();
            return person;
        }
        public Person DeletePerson(int id)
        {
            var person = db.Persons.Single(x => x.Id == id);
            db.Persons.Remove(person);
            db.SaveChanges();
            return person;
        }
       
    }
}
