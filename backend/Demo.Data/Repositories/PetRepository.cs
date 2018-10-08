using Demo.Shared.Data;
using Demo.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Data.Repositories
{
    public class PetRepository : RepositoryBase<Guid, Pet>, IPetRepository
    {
        public PetRepository(DemoDbContext context, ICache cache) : base(context, cache)
        { }

        public override void DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<Pet> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pet>> GetPetsByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pet>> GetPetsByOwnerId(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Pet>> GetRangeAsync(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        protected override string GetCacheKey(Pet entity)
        {
            return $"pet:{entity.Id}";
        }
    }
}
