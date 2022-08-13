using LiteDB;
using System.Linq.Expressions;

namespace StalkerMUD.Server.Data
{
    public class LiteDbRepository<T>: IRepository<T>
    {
        private readonly ILiteDatabase _db;

        public LiteDbRepository(ILiteDatabase db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return _db.GetCollection<T>().FindAll();
        }

        public async Task<T> GetAsync(int ID)
        {
            return _db.GetCollection<T>().FindById(ID);
        }

        public async Task<int> InsertAsync(T entity)
        {
            return _db.GetCollection<T>().Insert(entity);
        }

        public async Task<IEnumerable<T>> SelectAsync(Expression<Func<T, bool>> predicate)
        {
            return _db.GetCollection<T>().Find(predicate);
        }

        public async Task<T> SelectSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return _db.GetCollection<T>().FindOne(predicate) 
                ?? throw new KeyNotFoundException(typeof(T).Name);
        }
    }
}
