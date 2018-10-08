using AutoFixture;
using Demo.Shared.Data;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace Demo.Tests
{
    public class ApiClientTests : BaseTest
    {
        public ApiClientTests()
        {
            _correctUrl = "http://api.com/api";
        }

        [Fact]
        public void RequestAsync_CorrectApiAddress_ReturnsOkWithListOfPeople()
        {
            var json = JsonConvert.SerializeObject(_people);
            var req = new HttpRequestMessage(new HttpMethod("GET"), _correctUrl);

            throw new NotImplementedException();
        }

        [Fact]
        public void RequestAsync_IncorrectApiAddress_ThrowsException()
        {
            throw new NotImplementedException();
        }

        private string _correctUrl;
        private Mock<HttpClient> mockHttpClient;
    }
}
