using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Iris.Localization.AspNetCore.Old
{
    /// <summary>
    /// Baslangıc Lokalizasyon ekleme
    /// </summary>
    public static class LocalizerExtensions
    {
        private static readonly HcLocalizationOptions _options;

        public static IServiceCollection AddCustomLocalization(this IServiceCollection services, Action<HcLocalizationOptions>? opts = null)
        {
            if (opts != null)
            {
                opts?.Invoke(_options);
                services.Configure(opts);
            }

            services.ApplyCustomLocalizors();



            return services;
        }


        public static IApplicationBuilder UseCustomLocalization(this IApplicationBuilder builder)
        {
            //Genel culture ekleme


            builder.UseMiddleware<CustomLocalizationMiddleware>();

            return builder;
        }

        private static IServiceCollection ApplyCustomLocalizors(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, HcLocalizerFactory>();


            //if (_options.UseDatabase)
            //{

            //    //services.AddTransient<IStringLocalizer, DbLocalizer>();
            //}

            //if (_options.UseJsonFile)
            //{
            //    if (string.IsNullOrEmpty(_options.ResourcesPath))
            //        throw new NotImplementedException("ResourcePath required with UseJsonFile.");

            //    //services.AddTransient<IStringLocalizer, JsonLocalizer>();
            //}

            return services;
        }


    }


}