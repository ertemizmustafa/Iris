using Iris.Localization.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Iris.Localization.SqlServer
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddLocalizationDatabase(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string defaultSchema)
        {
            string connectionString = configuration.GetConnectionString(connectionStringName);

            LocalizationDbContext.SchemaName = defaultSchema;
            services.AddDbContext<LocalizationDbContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            return services;
        }
    }
}
