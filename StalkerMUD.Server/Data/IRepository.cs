using System.Linq.Expressions;

namespace StalkerMUD.Server.Data
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(int ID);

        Task<IEnumerable<T>> GetAllAsync();

        Task<int> InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task<T> SelectSingleAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> SelectAsync(Expression<Func<T, bool>> predicate);
    }
}
