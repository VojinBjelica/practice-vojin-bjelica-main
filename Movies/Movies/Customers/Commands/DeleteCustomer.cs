using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Customers.Commands
{
    public static class DeleteCustomer
    {
        [PublicAPI]
        public class Command : IRequest<bool>
        {
            public Guid CustomerId { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<Customer> _customerRepository;

            public RequestHandler(IRepository<Customer> customerRepository)
            {
                _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            }
            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var customer = _customerRepository.GetById(request.CustomerId);
                if (customer == null)
                {
                    return Task.FromResult(false);
                }

                _customerRepository.Delete(customer);
                _customerRepository.SaveChanges();

                return Task.FromResult(true);
            }
        }


    }
}