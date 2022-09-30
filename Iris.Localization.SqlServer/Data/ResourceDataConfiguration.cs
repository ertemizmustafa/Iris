using Iris.Localization.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iris.Localization.SqlServer.Data
{

    internal class ResourceDataConfiguration : IEntityTypeConfiguration<ResourceData>
    {
        public void Configure(EntityTypeBuilder<ResourceData> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.CultureName, x.Path });
            builder.Property(x => x.CreatedBy);
            builder.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
