using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Models;

namespace MovieStore.Infrastructure.Configurations
{
    public class LifelongMovieConfiguration : IEntityTypeConfiguration<LifelongMovie>
    {
        public void Configure(EntityTypeBuilder<LifelongMovie> builder)
        {
            builder.HasBaseType<Movie>();
        }
    }
}
