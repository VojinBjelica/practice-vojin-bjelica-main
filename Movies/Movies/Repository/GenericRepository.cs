using Microsoft.EntityFrameworkCore;
using MovieStore.Infrastructure.Data;
using System.Linq.Expressions;

namespace MovieStore.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly MovieStoreContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(MovieStoreContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity) => _dbSet.Add(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public IEnumerable<T> GetAll() => _dbSet.AsEnumerable();

        public virtual T? GetById(Guid id) => _dbSet.Find(id);

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate.Compile());

        public void SaveChanges() => _context.SaveChanges();

    }
}
