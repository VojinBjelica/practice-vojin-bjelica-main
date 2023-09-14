using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Customers.Commands
{
    public static class CreateCustomer
    {
        [PublicAPI]
        public class Command : IRequest<Customer>
        {
            public string Email { get; set; } = string.Empty;
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, Customer>
        {
            private readonly IRepository<Customer> _customerRepository;

            public RequestHandler(IRepository<Customer> customerRepository)
            {
                _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            }

            public Task<Customer> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var existingCustomer = _customerRepository.GetAll().SingleOrDefault(x => x.Email.Value == request.Email);
                if (existingCustomer != null)
                {
                    return Task.FromResult(existingCustomer);
                }

                var emailResult = Email.Create(request.Email);

                if (emailResult.IsFailed)
                {
                    return null;
                }

                var customer = new Customer(emailResult.Value);

                _customerRepository.Add(customer);
                _customerRepository.SaveChanges();

                return Task.FromResult(customer);
            }
        }
    }
}
