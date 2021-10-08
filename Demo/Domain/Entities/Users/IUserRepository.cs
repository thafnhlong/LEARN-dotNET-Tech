using Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public interface IUserRepository : IRepository<User>
    {
    }
}