using MVVM.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM.Services
{
    public class PersonService : IPersonService
    {

        private List<Person> people = new List<Person>();

        public PersonService()
        {
            people = new List<Person>() {
                    new Person { Name = "Anna", Age = 27 },
                    new Person { Name = "Christian", Age = 32 },
                    new Person { Name = "Helle", Age = 54 }
                };
        }

        public void CreatePerson(Person person) => people.Add(person);

        public void DeletePerson(Person person) => people.Remove(person);

        public List<Person> GetPeople() => people;
    }
}
