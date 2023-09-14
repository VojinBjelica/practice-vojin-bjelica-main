using JetBrains.Annotations;
using MediatR;
using MovieStore.Core.Models;
using MovieStore.Repository;

namespace MovieStore.Movies.Commands
{
    public static class CreateMovie
    {
        [PublicAPI]
        public class Command : IRequest
        {
            public string Name { get; set; } = string.Empty;
            public LicensingType LicensingType { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command>
        {
            private readonly IRepository<Movie> _movieRepository;

            public RequestHandler(IRepository<Movie> movieRepository)
            {
                _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            }

            public Task Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                Movie movie;
                if (request.LicensingType == LicensingType.TwoDay)
                {
                    movie = new TwoDayMovie(request.Name);
                }
                else
                {
                    movie = new LifelongMovie(request.Name);
                }


                _movieRepository.Add(movie);
                _movieRepository.SaveChanges();

                return Task.CompletedTask;
            }
        }
    }
}
