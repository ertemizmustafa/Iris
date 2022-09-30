using Iris.Localization.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Iris.Localization.SqlServer.Data
{
    public class LocalizationDbContext : DbContext
    {

        public static string SchemaName { get; set; }

        internal LocalizationDbContext(DbContextOptions<LocalizationDbContext> options) : base(options)
        {

        }

        public DbSet<ResourceData> ResourceDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
            }

            if (SchemaName != null)
                builder.HasDefaultSchema(SchemaName);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }

    internal class DbContextModelBuilder
    {
        public static IModel CreateModelBuilder(string defaultSchema)
        {
            var builder = new ModelBuilder();

            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
            }

            if (defaultSchema != null)
                builder.HasDefaultSchema(defaultSchema);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            return builder.FinalizeModel();
        }
    }
}
