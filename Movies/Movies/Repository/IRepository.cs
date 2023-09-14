using System.Linq.Expressions;

namespace MovieStore.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(Guid id);
        void Add(T entity);
        void Delete(T entity);
        void SaveChanges();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
