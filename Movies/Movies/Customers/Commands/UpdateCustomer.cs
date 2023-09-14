using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Customers.Commands
{
    public static class UpdateCustomer
    {
        [PublicAPI]
        public class Command : IRequest<bool>
        {
            public Guid CustomerId { get; set; }
            public string Email { get; set; } = string.Empty;
            public CustomerRole Role { get; set; }
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

                var emailResult = Email.Create(request.Email);

                if (emailResult.IsFailed)
                {
                    return null;
                }

                customer.Email = emailResult.Value;
                customer.Role = request.Role;
                _customerRepository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
