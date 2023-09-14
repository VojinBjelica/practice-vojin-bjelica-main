using FakeItEasy;
using FluentAssertions;
using MovieStore.Core.Models;
using MovieStore.Customers.Commands;
using MovieStore.Repository;

namespace MovieStoreTests
{
    public class PurchaseMovieTests
    {
        private readonly PurchaseMovie.RequestHandler _requestHandler;
        private readonly IRepository<PurchasedMovie> _purchasedMovieRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Movie> _movieRepository;

        public PurchaseMovieTests()
        {
            _customerRepository = A.Fake<IRepository<Customer>>();
            _purchasedMovieRepository = A.Fake<IRepository<PurchasedMovie>>();
            _movieRepository = A.Fake<IRepository<Movie>>();
            _requestHandler = new PurchaseMovie.RequestHandler(_customerRepository, _movieRepository, _purchasedMovieRepository);
        }

        [Fact]
        public void ArgumentNullTest()
        {
            Action action = () => _requestHandler.Handle(null!, CancellationToken.None);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldFailIfCustomerIsNull()
        {
            var customerGuid = Guid.NewGuid();
            var movie = new TwoDayMovie("Test");

            A.CallTo(() => _customerRepository.GetById(customerGuid)).Returns(null);
            A.CallTo(() => _movieRepository.GetById(movie.Id)).Returns(movie);

            var result = _requestHandler.Handle(new PurchaseMovie.Command { CustomerId = customerGuid, MovieId = movie.Id }, CancellationToken.None).Result;

            result.IsFailed.Should().Be(true);
            result.Errors.Single().Message.Should().Be("Customer or movie not found.");
        }

        [Fact]
        public void ShouldFailIfMovieIsNull()
        {
            var movieGuid = Guid.NewGuid();
            var customer = new Customer(Email.Create("test@gmail.com").Value);
            A.CallTo(() => _customerRepository.GetById(customer.Id)).Returns(customer);
            A.CallTo(() => _movieRepository.GetById(movieGuid)).Returns(null);

            var result = _requestHandler.Handle(new PurchaseMovie.Command { CustomerId = customer.Id, MovieId = movieGuid }, CancellationToken.None).Result;

            result.IsFailed.Should().Be(true);
            result.Errors.Single().Message.Should().Be("Customer or movie not found.");
        }

        [Fact]
        public void ShouldFailIfCustomerAlreadyOwnsTwoDayMovie()
        {
            var customer = new Customer(Email.Create("test@gmail.com").Value);
            var movie = new TwoDayMovie("Test");
            customer.PurchaseMovie(movie);
            A.CallTo(() => _customerRepository.GetById(customer.Id)).Returns(customer);
            A.CallTo(() => _movieRepository.GetById(movie.Id)).Returns(movie);

            var result = _requestHandler.Handle(new PurchaseMovie.Command { CustomerId = customer.Id, MovieId = movie.Id }, CancellationToken.None).Result;

            result.IsFailed.Should().Be(true);
            result.Errors.Single().Message.Should().Be("Customer already owns the movie.");
        }


        [Fact]
        public void ShouldFailIfCustomerAlreadyOwnsLifelongMovie()
        {
            var customer = new Customer(Email.Create("test@gmail.com").Value);
            var movie = new LifelongMovie("Test");
            customer.PurchaseMovie(movie);
            A.CallTo(() => _customerRepository.GetById(customer.Id)).Returns(customer);
            A.CallTo(() => _movieRepository.GetById(movie.Id)).Returns(movie);

            var result = _requestHandler.Handle(new PurchaseMovie.Command { CustomerId = customer.Id, MovieId = movie.Id }, CancellationToken.None).Result;

            result.IsFailed.Should().Be(true);
            result.Errors.Single().Message.Should().Be("Customer already owns the movie.");
        }

        [Fact]
        public void ShouldSuceedPurchasingLifelongMovie()
        {
            var customer = new Customer(Email.Create("test@gmail.com").Value);
            var movie = new LifelongMovie("Test");
            var purchasedMovieCount = customer.PurchasedMovies.Count;
            A.CallTo(() => _customerRepository.GetById(customer.Id)).Returns(customer);
            A.CallTo(() => _movieRepository.GetById(movie.Id)).Returns(movie);

            var result = _requestHandler.Handle(new PurchaseMovie.Command { CustomerId = customer.Id, MovieId = movie.Id }, CancellationToken.None).Result;

            result.IsSuccess.Should().Be(true);
            customer.PurchasedMovies.Should().HaveCount(purchasedMovieCount + 1);
            customer.PurchasedMovies.First().Movie.Should().Be(movie);
        }

        [Fact]
        public void ShouldSuceedPurchasingTwoDayMovie()
        {
            var customer = new Customer(Email.Create("test@gmail.com").Value);
            var movie = new TwoDayMovie("Test");
            A.CallTo(() => _customerRepository.GetById(customer.Id)).Returns(customer);
            A.CallTo(() => _movieRepository.GetById(movie.Id)).Returns(movie);

            var result = _requestHandler.Handle(new PurchaseMovie.Command { CustomerId = customer.Id, MovieId = movie.Id }, CancellationToken.None).Result;

            result.IsSuccess.Should().Be(true);

            customer.PurchasedMovies.Should().HaveCountGreaterThan(0);
            customer.PurchasedMovies.First().Movie.Should().Be(movie);
            customer.PurchasedMovies.First().ExpirationDate.Value.Should().BeCloseTo(DateTime.UtcNow.AddDays(2), precision: TimeSpan.FromMilliseconds(1000));
        }
    }
}
