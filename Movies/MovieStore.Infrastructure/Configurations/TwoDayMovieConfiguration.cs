using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Models;

namespace MovieStore.Infrastructure.Configurations
{
    public class TwoDayMovieConfiguration : IEntityTypeConfiguration<TwoDayMovie>
    {
        public void Configure(EntityTypeBuilder<TwoDayMovie> builder)
        {
            builder.HasBaseType<Movie>();
        }
    }
}
