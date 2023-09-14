using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Customers.Queries
{
    public static class GetAllCustomers
    {
        [PublicAPI]
        public class Query : IRequest<IEnumerable<Customer>> { }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Query, IEnumerable<Customer>>
        {
            private readonly IRepository<Customer> _customerRepository;

            public RequestHandler(IRepository<Customer> customerRepository)
            {
                _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            }

            public Task<IEnumerable<Customer>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var customers = _customerRepository.GetAll();

                return Task.FromResult(customers);
            }
        }

    }
}
