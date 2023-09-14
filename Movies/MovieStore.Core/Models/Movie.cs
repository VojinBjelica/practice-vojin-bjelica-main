using System.ComponentModel.DataAnnotations;

namespace MovieStore.Core.Models
{
    public abstract class Movie
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public LicensingType LicensingType { get; set; }

        public abstract Money CalculatePrice();

        public Movie(string name, LicensingType licensingType)
        {
            Id = Guid.NewGuid();
            Name = name;
            LicensingType = licensingType;
        }

        public abstract ExpirationDate CalculateExpirationDate();
    }
}
