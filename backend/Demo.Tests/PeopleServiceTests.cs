using Demo.Data.Services;
using Demo.Shared.Data;
using Demo.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Demo.Tests
{
    public class PeopleServiceTests : BaseTest
    {
        public PeopleServiceTests() : base()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockPersonRepo = new Mock<IPersonRepository>();
            _mockPetRepo = new Mock<IPetRepository>();
            _mockApiClient = new Mock<IHttpClient>();
            _peopleService = new PeopleService(_mockConfig.Object, _mockPersonRepo.Object, _mockPetRepo.Object, 
                _mockApiClient.Object);
        }

        [Fact]
        public async void GetPeopleAsync_DatabaseIsOk_ReturnsListOfPeople()
        {
            _mockPersonRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_people);

            var result = await _peopleService.GetPeopleAsync();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Person>>(result);
            Assert.Equal(_people, result);
        }

        [Fact]
        public async void GetPeopleAsync_NoData_ReturnsNull()
        {
            _mockPersonRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync((IEnumerable<Person>)null);

            var result = await _peopleService.GetPeopleAsync();

            Assert.Null(result);
        }

        private IPeopleService _peopleService;
        private Mock<IConfiguration> _mockConfig;
        private Mock<IPersonRepository> _mockPersonRepo;
        private Mock<IPetRepository> _mockPetRepo;
        private Mock<IHttpClient> _mockApiClient;
        
    }
}
