using Domain.Common;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DemoContext _context;
        public IUnitOfWork UnitOfWork { get { return _context; } }

        public UserRepository(DemoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> AddAsync(User entity)
        {
            return (await _context.Users.AddAsync(entity)).Entity;
        }

        public Task DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<User> GetByIdAsync(int id)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.id == id);
        }

        public Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}