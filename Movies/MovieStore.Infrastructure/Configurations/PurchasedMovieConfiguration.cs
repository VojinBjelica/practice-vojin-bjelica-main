using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Models;

namespace MovieStore.Infrastructure.Configurations
{
    public class PurchasedMovieConfiguration : IEntityTypeConfiguration<PurchasedMovie>
    {
        public void Configure(EntityTypeBuilder<PurchasedMovie> builder)
        {
            builder.ToTable("PurchasedMovies");
            builder.HasKey(pm => pm.Id);
            builder.Property(pm => pm.PurchaseDate).IsRequired();

            builder.HasOne(pm => pm.Movie)
                .WithMany();

            builder.HasOne(pm => pm.Customer)
                .WithMany(c => c.PurchasedMovies);

            builder.Property(pm => pm.ExpirationDate)
                .IsRequired(false)
                .HasConversion(v => v.Value, v => new ExpirationDate(v));
        }
    }
}
