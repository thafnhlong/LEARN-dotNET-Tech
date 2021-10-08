using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IRepository<T> where T : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
