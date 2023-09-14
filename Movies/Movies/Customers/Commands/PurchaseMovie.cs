using FluentResults;
using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Customers.Commands
{
    public static class PurchaseMovie
    {
        [PublicAPI]
        public class Command : IRequest<Result>
        {
            public Guid CustomerId { get; set; }
            public Guid MovieId { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, Result>
        {
            private readonly IRepository<Customer> _customerRepository;
            private readonly IRepository<Movie> _movieRepository;
            private readonly IRepository<PurchasedMovie> _purchasedMovieRepository;

            public RequestHandler(IRepository<Customer> customerRepository, IRepository<Movie> movieRepository, IRepository<PurchasedMovie> purchasedMovieRepository)
            {
                _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
                _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
                _purchasedMovieRepository = purchasedMovieRepository ?? throw new ArgumentNullException(nameof(purchasedMovieRepository));
            }
            public Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var customer = _customerRepository.GetById(request.CustomerId);
                var movie = _movieRepository.GetById(request.MovieId);

                if (customer == null || movie == null)
                {
                    return Task.FromResult(Result.Fail("Customer or movie not found."));
                }

                Result result = customer.PurchaseMovie(movie);
                if (result.IsFailed)
                {
                    return Task.FromResult(Result.Fail(result.Errors.Select(x => x.Message)));
                }

                _purchasedMovieRepository.SaveChanges();
                _customerRepository.SaveChanges();

                return Task.FromResult(Result.Ok());
            }
        }
    }
}
