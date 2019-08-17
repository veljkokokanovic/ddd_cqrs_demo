using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadModel.Repository.MsSql
{
    public abstract class RepositoryBase<TEntity, TId> : IReadModelRepository<TEntity, TId> where TEntity : Entity<TId>
    {
        protected RepositoryBase(IConfiguration config)
        {
            Configuration = config;
        }

        protected IConfiguration Configuration { get; private set; }

        public abstract Task<TEntity> GetAsync(TId id);
        public abstract Task SaveAsync(TEntity entity);
        public abstract Task DeleteAsync(TId id);
        public abstract Task<IEnumerable<TEntity>> GetAsync(params TId[] id);
        public abstract Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
