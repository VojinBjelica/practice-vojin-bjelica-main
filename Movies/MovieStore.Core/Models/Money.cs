using FluentResults;

namespace MovieStore.Core.Models
{
    public record Money
    {
        public double Value { get; set; }

        private Money(double value)
        {
            Value = value;
        }

        public static Result<Money> Create(double value)
        {
            if (value.Equals(null))
            {
                return Result.Fail("Money value cannot be null.");
            }

            if (value < 0)
            {
                return Result.Fail("Money value cannot be negative.");
            }

            if (Math.Abs(Math.Round(value, 2) - value) > 0.01)
            {
                return Result.Fail("Money value cannot have more than two decimal places.");
            }

            return Result.Ok(new Money(value));
        }

        public static Money Of(double value)
        {
            var result = Money.Create(value);
            if (result.IsFailed)
            {
                throw new InvalidOperationException();
            }
            return result.Value;
        }

        public static Money operator +(Money money, Money moreMoney)
        {
            return Create(money.Value + moreMoney.Value).Value;
        }

        public static bool operator <(Money money, Money moreMoney)
        {
            return money.Value < moreMoney.Value;
        }

        public static bool operator >(Money money, Money moreMoney)
        {
            return money.Value > moreMoney.Value;
        }
    }
}
