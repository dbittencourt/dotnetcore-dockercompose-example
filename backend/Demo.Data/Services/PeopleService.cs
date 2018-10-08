using Demo.Shared.Data;
using Demo.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Data.Services
{
    public class PeopleService : IPeopleService
    {
        public PeopleService(IConfiguration config, IPersonRepository personRepository, IPetRepository petRepository,
            IHttpClient apiClient)
        {
            _svcEndpoint = config["ApiEndpoints:People"];
            _personRepository = personRepository;
            _apiClient = apiClient;

            var dbIsPopulated = _personRepository.HasEntitiesAsync().Result;
            if (!dbIsPopulated)
            {
                // retrieves all persons and store them in the database
                var people = RetrievePeopleFromApi().Result;
                _personRepository.AddRangeAsync(people);
            }
        }

        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            var people = await _personRepository.GetAllAsync();

            return people;
        }

        public async Task<IEnumerable<Person>> GetPersonByNameAsync(string name)
        {
            var people = await _personRepository.GetPeopleByName(name);

            return people;
        }

        public Task<IEnumerable<Pet>> GetPersonPets(Guid personId)
        {
            var pets = _petRepository.GetPetsByOwnerId(personId);

            return pets;
        }

        private async Task<IEnumerable<Person>> RetrievePeopleFromApi()
        {
            var optional = new Dictionary<string, string>();

            var people = await _apiClient.RequestAsync<List<Person>>(_svcEndpoint, optional, false);
            return people;
        }

        private readonly IHttpClient _apiClient;
        private readonly IPersonRepository _personRepository;
        private readonly IPetRepository _petRepository;
        private string _svcEndpoint;
    }
}
