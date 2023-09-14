using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Movies.Commands
{
    public static class UpdateMovie
    {
        [PublicAPI]
        public class Command : IRequest<bool>
        {
            public Guid MovieId { get; set; }
            public string Name { get; set; } = string.Empty;
            public LicensingType LicensingType { get; set; }
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
                Movie updatedMovie = request.LicensingType == LicensingType.Lifelong ? new LifelongMovie(movie.Name) { Id = movie.Id } : new TwoDayMovie(movie.Name) { Id = movie.Id };
                _movieRepository.Add(updatedMovie);
                _movieRepository.SaveChanges();

                /* movie.Name = request.Name;
                 movie.LicensingType = request.LicensingType;
                 _movieRepository.SaveChanges();*/

                return Task.FromResult(true);
            }
        }
    }
}
