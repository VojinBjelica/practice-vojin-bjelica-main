namespace MovieStore.Core.Models
{
    public class TwoDayMovie : Movie
    {
        public TwoDayMovie(string name) : base(name, LicensingType.TwoDay) { }

        public override ExpirationDate CalculateExpirationDate()
        {
            return new ExpirationDate(DateTime.UtcNow.AddDays(2));
        }
        public override Money CalculatePrice()
        {
            return Money.Of(5);
        }
    }
}
