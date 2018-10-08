using Demo.Shared.Data;
using Demo.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Data.Repositories
{
    public class PersonRepository : RepositoryBase<Guid, Person>, IPersonRepository
    {
        public PersonRepository(DemoDbContext context, ICache cache) : base(context, cache)
        {
            context.Database.EnsureCreated();
        }

        public override void DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<Person> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Person>> GetAllAsync()
        {
            var entities = _cache.GetValues<Person>(typeof(Person).Name);
            if (entities == null || entities.Count() < 1)
            {
                entities = await _entities.AsNoTracking().Include(person => person.Pets).ToListAsync();

                foreach (var entity in entities)
                    _cache.SetValue(GetCacheKey(entity), entity);
            }

            return entities;
        }

        public async Task<IEnumerable<Person>> GetPeopleByName(string name)
        {
            var people = _cache.GetValues<Person>(typeof(Person).Name)
                .Where(person => (person as Person).Name.ToLower().Contains(name.ToLower()));

            if (people == null || people.Count() < 1)
            {
                people = await _entities.AsNoTracking()
                .Where(person => person.Name.ToLower().Contains(name.ToLower()))
                .Include(person => person.Pets)
                .ToListAsync();

                foreach (var person in people)
                    _cache.SetValue(GetCacheKey(person), person);
            }

            return people;
        }

        public override Task<IEnumerable<Person>> GetRangeAsync(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        protected override string GetCacheKey(Person entity)
        {
            return $"Person:{entity.PersonId}";
        }
    }
}
