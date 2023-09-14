using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Customers.Queries
{
    public static class GetCustomerById
    {
        [PublicAPI]
        public class Query : IRequest<Customer>
        {
            public Guid Id { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Query, Customer>
        {
            private readonly IRepository<Customer> _customerRepository;

            public RequestHandler(IRepository<Customer> customerRepository)
            {
                _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            }
            public Task<Customer> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }
                var customer = _customerRepository.GetById(request.Id);

                if (customer == null)
                {
                    throw new NullReferenceException(nameof(customer));
                }

                return Task.FromResult(customer);
            }
        }
    }
}
