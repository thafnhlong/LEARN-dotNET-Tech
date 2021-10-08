using Domain.Common;
using Newtonsoft.Json;

namespace Domain.Entities.Roles
{
    public class Role : Entity, IAggregateRoot
    {
        public string code { get; set; }
        public string description { get; set; }
    }
}