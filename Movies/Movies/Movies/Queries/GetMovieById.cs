using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Movies.Queries
{
    public static class GetMovieById
    {

        [PublicAPI]
        public class Query : IRequest<Movie?>
        {
            public Guid MovieId { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Query, Movie?>
        {
            private readonly IRepository<Movie> _movieRepository;

            public RequestHandler(IRepository<Movie> movieRepository)
            {
                _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            }
            public Task<Movie?> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var movie = _movieRepository.GetById(request.MovieId);

                return Task.FromResult(movie);
            }
        }
    }
}
