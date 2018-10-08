using System;
using System.Collections.Generic;

namespace Demo.Shared.Data
{
    public class Person
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public IEnumerable<Pet> Pets { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
