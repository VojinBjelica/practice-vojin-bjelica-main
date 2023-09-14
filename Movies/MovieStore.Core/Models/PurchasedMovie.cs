using System.ComponentModel.DataAnnotations;

namespace MovieStore.Core.Models
{
    public class PurchasedMovie
    {
        public Guid Id { get; set; }
        public Movie Movie { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
        public ExpirationDate ExpirationDate { get; set; }
        public Customer Customer { get; set; }

        public PurchasedMovie(Movie movie, Customer customer)
        {
            PurchaseDate = DateTime.UtcNow;
            ExpirationDate = movie.CalculateExpirationDate();
            Movie = movie;
            Customer = customer;
        }

        private PurchasedMovie() { }
    }
}
