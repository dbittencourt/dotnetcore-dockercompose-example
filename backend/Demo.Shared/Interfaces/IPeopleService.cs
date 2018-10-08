using Demo.Shared.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Shared.Interfaces
{
    public interface IPeopleService
    {
        Task<IEnumerable<Person>> GetPeopleAsync();

        Task<IEnumerable<Person>> GetPersonByNameAsync(string name);

        Task<IEnumerable<Pet>> GetPersonPets(Guid personId);
    }
}
