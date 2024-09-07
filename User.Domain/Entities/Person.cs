using BaseShare.Common.Domain;

namespace User.Domain.Entities
{
    public class Person : EntityBase
    {

        public string Name { get; private set; } = null!;

        public string Email { get; private set; } = null!;


        // Parameterless constructor needed by EF Core
        private Person() { }

        public Person(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Person name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Person email cannot be empty.", nameof(name));

            Name = name;

        }


    }
}
