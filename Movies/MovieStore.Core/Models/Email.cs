using FluentResults;
using System.Text.RegularExpressions;

namespace MovieStore.Core.Models
{
    public record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Result.Fail("Email value cannot be null or empty.");
            }

            var emailRegex = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w+$");
            if (!emailRegex.IsMatch(value))
            {
                return Result.Fail("Invalid email format.");
            }

            return Result.Ok(new Email(value));
        }
    }
}
