using Domain.Common;
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class DemoContext : DbContext, IUnitOfWork
    {
        public DbSet<Role> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }

        public DemoContext(DbContextOptions<DemoContext> options) : base(options)
        {
            System.Diagnostics.Debug.WriteLine("DemoContext::ctor ->" + this.GetHashCode());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            //await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers)
            // performed through the DbContext will be committed
            try {
                await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) {
                throw ex;
            }

            return true;
        }
    }
}