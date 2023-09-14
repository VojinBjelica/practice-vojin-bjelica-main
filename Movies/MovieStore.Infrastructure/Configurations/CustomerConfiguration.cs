using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Models;

namespace MovieStore.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Email)
                .HasConversion(v => v.Value,
                v => Email.Create(v).Value);
            //builder.OwnsOne(c => c.Email, email =>
            //{
            //    email.Property(p => p.Value)
            //    .HasColumnName("Email")
            //    .IsRequired();
            //});

            builder.Property(c => c.MoneySpent)
                .HasConversion(v => v.Value,
                v => Money.Create(v).Value);


            builder.OwnsOne(c => c.CustomerStatus, customerStatus =>
            {
                customerStatus.Property(cs => cs.Status)
                .HasColumnName("Status")
                .IsRequired();

                customerStatus.OwnsOne(cs => cs.StatusExpirationDate, statusExpirationDate =>
                {
                    statusExpirationDate.Property(sed => sed.Value)
                    .HasColumnName("StatusExpirationDate");
                });

            });
        }
    }
}
