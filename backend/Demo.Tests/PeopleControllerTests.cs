using AutoFixture;
using Demo.Controllers;
using Demo.Shared.Data;
using Demo.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Demo.Tests
{
    public class PeopleControllerTests : BaseTest
    {
        public PeopleControllerTests() : base()
        {
            _mockPeopleService = new Mock<IPeopleService>();
            _controller = new PeopleController(_mockPeopleService.Object);
        }

        [Fact]
        public async void GetAllPeople_ApiExists_ReturnsOkWithListOfPeople()
        {
            _mockPeopleService.Setup(svc => svc.GetPeopleAsync()).Returns(Task.FromResult(_people));

            var result = await _controller.GetAllPeople();

            Assert.NotNull(result);
            var methodResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<Person>>(methodResult.Value);

            Assert.Equal(_people, data);
        }

        [Fact]
        public async void GetAllPeople_ApiNotWorking_ReturnsNotFound()
        {
            _mockPeopleService.Setup(svc => svc.GetPeopleAsync()).Returns(Task.FromResult((IEnumerable<Person>)null));

            var result = await _controller.GetAllPeople();

            Assert.NotNull(result);
            var methodResult = Assert.IsType<NotFoundResult>(result);
        }

        private PeopleController _controller;
        private Mock<IPeopleService> _mockPeopleService;
    }
}
