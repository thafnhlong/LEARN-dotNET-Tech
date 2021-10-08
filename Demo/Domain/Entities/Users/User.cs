using Domain.Common;
using Domain.Entities.Roles;
using Newtonsoft.Json;

namespace Domain.Entities.Users
{
    public class User : Entity, IAggregateRoot
    {
        [JsonIgnore]
        public string password { get; private set; }

        public Role role { get; set; }

        public string username { get; set; }

        public void setPassword(string password)
        {
            this.password = BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool verifyPassword(string input)
        {
            return BCrypt.Net.BCrypt.Verify(input, password);
        }
    }
}