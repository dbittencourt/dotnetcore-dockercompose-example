using Demo.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Data
{
    public abstract class RepositoryBase<TKey, TModel> : IRepository<TKey, TModel> where TModel : class
    {
        public RepositoryBase(DemoDbContext context, ICache cache)
        {
            _context = context;
            _cache = cache;

            // removes old cache entries
            _cache.Clear(typeof(TModel).Name);
            _entities = _context.Set<TModel>();
        }

        public async void AddAsync(TModel entity)
        {
            if (entity != null)
            {
                await _entities.AddAsync(entity);
                await _context.SaveChangesAsync();

                _cache.SetValue(GetCacheKey(entity), entity);
            }
        }

        public async void AddRangeAsync(IEnumerable<TModel> entities)
        {
            if (entities != null)
            {
                await _entities.AddRangeAsync(entities);
                await _context.SaveChangesAsync();

                foreach(var entity in entities)
                    _cache.SetValue(GetCacheKey(entity), entity);
            }
        }

        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var entities = _cache.GetValues<TModel>(typeof(TModel).Name);
            if (entities == null || entities.Count() < 1)
            {
                entities = await _entities.AsNoTracking().ToListAsync();

                foreach (var entity in entities)
                    _cache.SetValue(GetCacheKey(entity), entity);
            }

            return entities;
        }

        public void Update(TModel entity)
        {
            if (entity != null)
            {
                _entities.Update(entity);
                _context.SaveChangesAsync();

                _cache.SetValue(GetCacheKey(entity), entity);
            }
        }

        public async Task<bool> HasEntitiesAsync()
        {
            return await _entities.CountAsync() > 0;
        }

        public abstract Task<TModel> GetAsync(TKey id);

        public abstract Task<IEnumerable<TModel>> GetRangeAsync(IEnumerable<TKey> ids);

        public abstract void DeleteAsync(TKey id);

        protected abstract string GetCacheKey(TModel entity);

        protected readonly DemoDbContext _context;
        protected readonly ICache _cache;
        protected DbSet<TModel> _entities;
    }
}
