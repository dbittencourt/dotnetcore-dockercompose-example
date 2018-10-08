using Demo.Shared.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Shared.Interfaces
{
    public interface IPetRepository : IRepository<Guid, Pet>
    {
        Task<IEnumerable<Pet>> GetPetsByName(string name);

        Task<IEnumerable<Pet>> GetPetsByOwnerId(Guid id);
    }
}
