using Microsoft.Extensions.Options;
using MovieStore.Core.Models;
using MovieStore.Infrastructure;
using MovieStore.Infrastructure.Configurations;
using MovieStore.Repository;
using System.Diagnostics;

namespace MovieStore.HostedService
{
    public class MovieExpirationService : IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly EmailServiceOptions _emailOptions;
        private readonly BackgroundServiceOptions _backgroundServiceOptions;
        private readonly IServiceProvider _serviceProvider;


        public MovieExpirationService(IServiceProvider serviceProvider, IOptions<EmailServiceOptions> emailOptions, IOptions<BackgroundServiceOptions> backgroundServiceOptions)
        {
            _serviceProvider = serviceProvider;
            _emailOptions = emailOptions.Value;
            _backgroundServiceOptions = backgroundServiceOptions.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine("STARTED BACKGROUND SERVICE**********************************");
            _timer = new Timer(CheckExpiredMovies, null, TimeSpan.Zero, TimeSpan.FromSeconds(_backgroundServiceOptions.CheckIntervalSeconds));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async void CheckExpiredMovies(object state)
        {
            Debug.WriteLine("New iteration started");
            using (var scope = _serviceProvider.CreateScope())
            {
                var purchasedMovieRepository = scope.ServiceProvider.GetRequiredService<IRepository<PurchasedMovie>>();
                var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                var expiredMovies = purchasedMovieRepository.Find(purchasedMovie => purchasedMovie.Movie is TwoDayMovie && purchasedMovie.ExpirationDate.IsExpired && purchasedMovie.ExpirationDate.Value > DateTime.UtcNow.AddSeconds(-_backgroundServiceOptions.CheckIntervalSeconds));

                foreach (var expiredMovie in expiredMovies)
                {
                    string subject = "Movie Expiration Notification";
                    string body = $"The movie '{expiredMovie.Movie.Name}' has expired.";

                    await emailService.SendEmailAsync(_emailOptions.SmtpUsername, expiredMovie.Customer.Email.Value, subject, body);

                }

            }
        }
    }
}
