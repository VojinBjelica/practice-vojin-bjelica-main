using FluentResults;
using System.ComponentModel.DataAnnotations;

namespace MovieStore.Core.Models
{
    public class Customer
    {
        public CustomerStatus CustomerStatus { get; set; }

        public Guid Id { get; }

        public Email Email { get; set; } = null!;

        public IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies;
        [Required]
        public CustomerRole Role { get; set; }

        public Money MoneySpent { get; private set; }

        private readonly List<PurchasedMovie> _purchasedMovies = new List<PurchasedMovie>();

        public Customer(Email email)
        {
            _purchasedMovies = new List<PurchasedMovie>();
            CustomerStatus = CustomerStatus.Create(Status.Regular, ExpirationDate.Infinity).Value;
            Role = CustomerRole.Regular;
            MoneySpent = Money.Of(0);
            Email = email;
        }

        public Result PurchaseMovie(Movie movie)
        {
            //TODO: Rijesiti se uslova apstraktnim klasama
            if (PurchasedMovies.Any(purcahsedMovie => purcahsedMovie.Movie != null && purcahsedMovie.Movie.Id == movie.Id && !purcahsedMovie.Movie.CalculateExpirationDate().IsExpired))
            {
                return Result.Fail("Customer already owns the movie.");
            }

            var moneySpent = CustomerStatus.IsAdvanced ? movie.CalculatePrice().Value * 0.7 : movie.CalculatePrice().Value;

            MoneySpent += Money.Create(moneySpent).Value;

            var purchasedMovie = new PurchasedMovie(movie, this);

            _purchasedMovies.Add(purchasedMovie);

            return Result.Ok();
        }

        public Result PromoteCustomer()
        {
            if (CustomerStatus.IsAdvanced || PurchasedMovies.Where(m => m.PurchaseDate >= DateTime.Now.AddMonths(-1)).Count() < 2)
            {
                return Result.Fail("Customer does not meet promotion criteria");
            }

            if (MoneySpent < Money.Of(20))
            {
                return Result.Fail("Customer hasn't spent enough money to qualify for promotion");
            }

            CustomerStatus = CustomerStatus.Create(Status.Advanced, new ExpirationDate(DateTime.UtcNow.AddYears(1))).Value;
            return Result.Ok();
        }
    }
}