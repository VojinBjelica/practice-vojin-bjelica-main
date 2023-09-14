using FakeItEasy;
using FluentAssertions;
using MovieStore.Core.Models;
using MovieStore.Customers.Commands;
using MovieStore.Repository;

namespace MovieStoreTests
{
    public class PromoteCustomerTests
    {
        private PromoteCustomer.RequestHandler _requestHandler;
        private IRepository<Customer> _customerRepository;

        public PromoteCustomerTests()
        {
            _customerRepository = A.Fake<IRepository<Customer>>();
            _requestHandler = new PromoteCustomer.RequestHandler(_customerRepository);
        }

        [Fact]
        public void ArgumentNullTest()
        {
            Action action = () => _requestHandler.Handle(null, CancellationToken.None);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldFailIfCustomerIsNull()
        {
            var someGuid = Guid.NewGuid();
            A.CallTo(() => _customerRepository.GetById(someGuid)).Returns(null);

            var result = _requestHandler.Handle(new PromoteCustomer.Command { CustomerId = someGuid }, CancellationToken.None).Result;

            result.IsFailed.Should().Be(true);
            result.Errors.Single().Message.Should().Be("Failed to promote customer");
        }

        [Fact]
        public void ShouldFailIfCustomerIsAdvanced()
        {
            var customer = new Customer(Email.Create("test@gmail.com").Value);
            var movie = new LifelongMovie("Test");
            var anotherMovie = new LifelongMovie("Test2");
            //customer.Id = Guid.NewGuid();
            customer.CustomerStatus = CustomerStatus.Create(Status.Advanced, new ExpirationDate(DateTime.UtcNow.AddDays(5))).Value;
            customer.PurchaseMovie(movie);
            customer.PurchaseMovie(anotherMovie);
            A.CallTo(() => _customerRepository.GetById(customer.Id)).Returns(customer);

            var result = _requestHandler.Handle(new PromoteCustomer.Command { CustomerId = customer.Id }, CancellationToken.None).Result;

            result.IsFailed.Should().Be(true);
            result.Errors.Single().Message.Should().Be("Customer does not meet promotion criteria");
        }

        [Fact]
        public void ShouldFailIfCustomerHasntPurchasedEnoughMovies()
        {
            var customer = new Customer(Email.Create("test@gmail.com").Value);
            //customer.Id = Guid.NewGuid();
            //customer.CustomerStatus = CustomerStatus.Create(Status.Regular, ExpirationDate.Infinity).Value;
            A.CallTo(() => _customerRepository.GetById(customer.Id)).Returns(customer);

            var result = _requestHandler.Handle(new PromoteCustomer.Command { CustomerId = customer.Id }, CancellationToken.None).Result;

            result.IsFailed.Should().Be(true);
            result.Errors.Single().Message.Should().Be("Customer does not meet promotion criteria");
        }

        [Fact]
        public void ShouldSucceed()
        {
            var customer = new Customer(Email.Create("test@gmail.com").Value);
            //customer.Id = Guid.NewGuid();
            var movie = new LifelongMovie("Test");
            var anotherMovie = new LifelongMovie("Test2");
            customer.PurchaseMovie(movie);
            customer.PurchaseMovie(anotherMovie);
            A.CallTo(() => _customerRepository.GetById(customer.Id)).Returns(customer);

            var result = _requestHandler.Handle(new PromoteCustomer.Command { CustomerId = customer.Id }, CancellationToken.None).Result;

            result.IsSuccess.Should().Be(true);
            customer.CustomerStatus.Status.Should().Be(Status.Advanced);
            customer.CustomerStatus.StatusExpirationDate.Value.Should().BeCloseTo(DateTime.UtcNow.AddYears(1), precision: TimeSpan.FromMilliseconds(1000));
        }
    }
}