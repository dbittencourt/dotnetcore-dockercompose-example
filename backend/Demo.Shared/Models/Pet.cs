using System;

namespace Demo.Shared.Data
{
    public class Pet
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public Animal type { get; set; }
    }

    public enum Animal
    {
        Cat,
        Dog,
        Fish,
        Kangaroo,
        Gremlin,
        Unknown
    }
}
