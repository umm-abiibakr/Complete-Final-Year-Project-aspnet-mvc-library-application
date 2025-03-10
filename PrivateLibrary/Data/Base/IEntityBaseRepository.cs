using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrivateLibrary.Data.Base
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase
    {
        Task AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(int id, T entity);
    }
}
