using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Models;

namespace MovieStore.Infrastructure.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired();

            builder.HasDiscriminator(m => m.LicensingType)
               .HasValue<LifelongMovie>(LicensingType.Lifelong)
               .HasValue<TwoDayMovie>(LicensingType.TwoDay);
        }
    }
}
