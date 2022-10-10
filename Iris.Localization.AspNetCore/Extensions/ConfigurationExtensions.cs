using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Iris.Localization.AspNetCore.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddIrisLocalization(this IServiceCollection services, Action<IrisLocalizationOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            AddLocalizationServices(services);

            services.Configure(setupAction);

            return services;
        }


        internal static void AddLocalizationServices(IServiceCollection services)
        {
            services.TryAddSingleton<IStringLocalizerFactory, IrisLocalizerFactory>();
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            //services.TryAddTransient(typeof(IStringLocalizer), typeof(StringLocalizer));
            //services.TryAddTransient(typeof(IStringLocalizer<string>), typeof(StringLocalizer<>));
            // services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));



        }

    }
}
