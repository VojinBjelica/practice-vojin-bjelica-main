namespace MovieStore.Core.Models
{
    public class LifelongMovie : Movie
    {
        public LifelongMovie(string name) : base(name, LicensingType.Lifelong) { }


        public override ExpirationDate CalculateExpirationDate()
        {
            return ExpirationDate.Infinity;
        }

        public override Money CalculatePrice()
        {
            return Money.Of(12);
        }
    }
}
