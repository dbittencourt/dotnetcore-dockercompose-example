using Demo.Shared.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Shared.Interfaces
{
    public interface IPersonRepository : IRepository<Guid, Person>
    {
        Task<IEnumerable<Person>> GetPeopleByName(string name);
    }
}
