using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
        public abstract Task<IQueryable<TEntity>> GetAsync(params TId[] id);
        public abstract Task SaveAsync(TEntity entity);
        public abstract Task DeleteAsync(TId id);
        public abstract Task<IQueryable<TEntity>> FindAsync(Func<TEntity, bool> predicate);
    }
}
