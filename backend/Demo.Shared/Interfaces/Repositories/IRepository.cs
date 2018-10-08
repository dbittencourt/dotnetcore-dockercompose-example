using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Shared.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> GetAsync(K id);
        Task<IEnumerable<T>> GetRangeAsync(IEnumerable<K> ids);
        Task<IEnumerable<T>> GetAllAsync();
        void AddAsync(T entity);
        void AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void DeleteAsync(K id);
        Task<bool> HasEntitiesAsync();
    }
}
