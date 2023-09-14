using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Models;
using MovieStore.Infrastructure.Data;

namespace MovieStore.Repository
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(MovieStoreContext context) : base(context)
        {
        }

        public override Customer? GetById(Guid id)
        {
            return _dbSet.Include(customer => customer.PurchasedMovies).FirstOrDefault(customer => customer.Id == id);
        }
    }
}
