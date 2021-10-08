using Domain.Common;
using Domain.Entities.Roles;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DemoContext _context;
        public IUnitOfWork UnitOfWork { get { return _context; } }

        public RoleRepository(DemoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Role> AddAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}