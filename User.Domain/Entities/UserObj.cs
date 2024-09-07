using BaseShare.Common.Domain;

namespace User.Domain.Entities
{
    public class UserObj : EntityBase
    {
        public string Password { get; private set; } = null!;
        public long PersonId { get; private set; }

        public Person? Person { get; private set; }

        private UserObj() { }

        public UserObj(string password, Person person )
        {
            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("Password cannot be empty.", nameof(Password));

            Person = person;
        }
        public void SetHashPassword(string crypto)
        {
            Password = crypto;
        }

    }
}
