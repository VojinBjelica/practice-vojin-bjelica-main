using FluentResults;
using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Customers.Commands
{
    public static class PromoteCustomer
    {
        [PublicAPI]
        public class Command : IRequest<Result>
        {
            public Guid CustomerId { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, Result>
        {
            private readonly IRepository<Customer> _customerRepository;

            public RequestHandler(IRepository<Customer> customerRepository)
            {
                _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            }

            public Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var customer = _customerRepository.GetById(request.CustomerId);

                if (customer == null)
                {
                    return Task.FromResult(Result.Fail("Failed to promote customer"));
                }
                //TODO customer can be promoted when:
                //1. not already advanced
                //2. purchased at least 2 movies in the last month
                //3. This will be added later once we introduce pricing, but must have spent at least X amount of money

                var result = customer.PromoteCustomer();
                if (result.IsFailed)
                {
                    return Task.FromResult(Result.Fail(result.Errors.Select(x => x.Message)));
                }

                _customerRepository.SaveChanges();

                return Task.FromResult(Result.Ok());
            }
        }
    }
}
