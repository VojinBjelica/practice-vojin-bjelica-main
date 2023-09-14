using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Movies.Queries
{
    public static class GetAllMovies
    {
        [PublicAPI]
        public class Query : IRequest<IEnumerable<Response>> { }

        [PublicAPI]
        public class Response
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public LicensingType LicensingType { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Query, IEnumerable<Response>>
        {
            private readonly IRepository<Movie> _movieRepository;

            public RequestHandler(IRepository<Movie> movieRepository)
            {
                _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            }
            public Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var movies = _movieRepository.GetAll();

                return Task.FromResult(movies.Select(movie => new Response
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    LicensingType = movie.LicensingType
                }));
            }
        }
    }
}
