namespace MovieStore.Core.Models
{
    public record ExpirationDate(DateTime? Value)
    {
        public static ExpirationDate Infinity => new(null as DateTime?);
        public bool IsExpired => this != Infinity && Value!.Value <= DateTime.UtcNow;
    }
}
