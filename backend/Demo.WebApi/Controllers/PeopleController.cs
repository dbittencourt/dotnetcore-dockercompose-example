using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Shared.Data;
using Demo.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        public PeopleController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        /// <summary>
        /// Returns a list of people and their pets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Person>> GetAllPeople(string q)
        {
            try 
            {
                IEnumerable<Person> people;
                if (string.IsNullOrWhiteSpace(q))
                    people = await _peopleService.GetPeopleAsync();
                else 
                    people = await _peopleService.GetPersonByNameAsync(q);
                    
                if (people != null && people.Count() > 0)
                    return Ok(people);
                else
                    return NotFound();
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
        }

        private readonly IPeopleService _peopleService;
    }
}
