using System.Linq.Expressions;
using System.Reflection;

namespace StalkerMUD.Server.Data
{
    public class MemoryRepository<T> : IRepository<T>
    {
        private readonly List<T> _memory;
        private readonly PropertyInfo _idProperty;

        public MemoryRepository(List<T> memory)
        {
            _memory = memory;

            var type = typeof(T);
            _idProperty = type.GetProperty("Id") ?? throw new ArgumentNullException("T must have a 'Id' int property!");
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return _memory;
        }

        public Task<T> GetAsync(int ID)
        {
            return SelectSingleAsync(x => ((int)_idProperty.GetValue(x)) == ID);
        }

        public async Task<int> InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> SelectAsync(Expression<Func<T, bool>> predicate)
        {
            var f = predicate.Compile();
            return _memory.FindAll(x => f(x));
        }

        public async Task<T> SelectSingleAsync(Expression<Func<T, bool>> predicate)
        {
            var all = await SelectAsync(predicate);
            return all.Single();
        }
    }
}
