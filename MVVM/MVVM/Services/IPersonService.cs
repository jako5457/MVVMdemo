using MVVM.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM.Services
{
    public interface IPersonService
    {

        List<Person> GetPeople();

        void CreatePerson(Person person);

        void DeletePerson(Person person);

    }
}
