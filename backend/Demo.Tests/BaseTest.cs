using AutoFixture;
using Demo.Shared.Data;
using System.Collections.Generic;

namespace Demo.Tests
{
    public abstract class BaseTest
    {
        public BaseTest()
        {
            _fixture = new Fixture();
            _people = _fixture.Build<Person>().CreateMany(10);
        }

        protected Fixture _fixture;
        protected IEnumerable<Person> _people;
    }
}
