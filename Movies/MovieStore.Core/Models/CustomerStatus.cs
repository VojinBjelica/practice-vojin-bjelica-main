using FluentResults;

namespace MovieStore.Core.Models
{
    public record CustomerStatus
    {
        public Status Status { get; }
        public ExpirationDate StatusExpirationDate { get; }

        private CustomerStatus() { }

        private CustomerStatus(Status status, ExpirationDate statusExpirationDate)
        {
            Status = status;
            StatusExpirationDate = statusExpirationDate;
        }

        public static Result<CustomerStatus> Create(Status status, ExpirationDate expirationDate)
        {
            if (status == Status.Advanced && expirationDate == ExpirationDate.Infinity)
            {
                return Result.Fail("StatusExpirationDate must be provided for Advanced status");
            }

            if (status == Status.Regular && expirationDate != ExpirationDate.Infinity)
            {
                return Result.Fail("Regular status cannot expire");
            }

            return Result.Ok(new CustomerStatus(status, expirationDate));
        }

        public bool IsAdvanced => Status == Status.Advanced && !StatusExpirationDate.IsExpired;
    }
}
