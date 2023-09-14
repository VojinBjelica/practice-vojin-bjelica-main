using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Models;
using MovieStore.Infrastructure.Data;
using System.Linq.Expressions;

namespace MovieStore.Repository
{
    public class PurchasedMovieRepository : GenericRepository<PurchasedMovie>
    {
        public PurchasedMovieRepository(MovieStoreContext context) : base(context)
        {
        }

        public override IEnumerable<PurchasedMovie> Find(Expression<Func<PurchasedMovie, bool>> predicate)
        {
            return _dbSet.Include(purchasedMovie => purchasedMovie.Movie).Include(purchasedMovie => purchasedMovie.Customer).Where(predicate.Compile());
        }
    }
}
