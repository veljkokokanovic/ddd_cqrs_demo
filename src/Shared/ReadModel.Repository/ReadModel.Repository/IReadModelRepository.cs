using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadModel.Repository
{
    public interface IReadModelRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        Task<TEntity> GetAsync(TId id);

        Task<IQueryable<TEntity>> GetAsync(params TId[] id);

        Task SaveAsync(TEntity entity);

        Task DeleteAsync(TId id);

        Task<IQueryable<TEntity>> FindAsync(Func<TEntity, bool> predicate);
    }
}
