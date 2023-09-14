using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Movies.Commands
{
    public static class DeleteMovie
    {
        [PublicAPI]
        public class Command : IRequest<bool>
        {
            public Guid MovieId { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<Movie> _movieRepository;

            public RequestHandler(IRepository<Movie> movieRepository)
            {
                _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            }
            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var movie = _movieRepository.GetById(request.MovieId);
                if (movie == null)
                {
                    return Task.FromResult(false);
                }

                _movieRepository.Delete(movie);
                _movieRepository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
