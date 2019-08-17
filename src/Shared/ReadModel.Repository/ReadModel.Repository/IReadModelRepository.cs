using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadModel.Repository
{
    public interface IReadModelRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        Task<TEntity> GetAsync(TId id);

        Task<IEnumerable<TEntity>> GetAsync(params TId[] id);

        Task SaveAsync(TEntity entity);

        Task DeleteAsync(TId id);

        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
