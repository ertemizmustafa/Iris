using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;

namespace Iris.Localization.AspNetCore
{

    /// <summary>
    /// Json yada Db localizor nesnesi urettir
    /// </summary>
    public class CustomLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IResourceNamesCache _resourceNamesCache = new ResourceNamesCache();
        private readonly ConcurrentDictionary<string, CustomStringLocalizer> _localizerCache = new ConcurrentDictionary<string, CustomStringLocalizer>();
        private readonly Dictionary<string, string> _sourceArgs;
        private readonly ILoggerFactory _loggerFactory;

        public CustomLocalizerFactory()
        {

        }

        public IStringLocalizer Create(Type resourceSource)
        {


            throw new NotImplementedException();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Filedan cek
    /// </summary>
    public class JsonLocalizer : IStringLocalizer
    {


        public JsonLocalizer()
        {

        }

        public LocalizedString this[string name] => throw new NotImplementedException();

        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Dbden cek
    /// </summary>
    public class DbLocalizer : IStringLocalizer
    {


        public DbLocalizer()
        {

        }

        public LocalizedString this[string name] => throw new NotImplementedException();

        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Baslangıc Lokalizasyon ekleme
    /// </summary>
    public static class LocalizerExtensions
    {
        private static readonly CustomLocalizationOptions _options;

        public static IServiceCollection AddCustomLocalization(this IServiceCollection services, Action<CustomLocalizationOptions>? opts = null)
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
            services.AddSingleton<IStringLocalizerFactory, CustomLocalizerFactory>();


            if (_options.UseDatabase)
            {

                services.AddTransient<IStringLocalizer, DbLocalizer>();
            }

            if (_options.UseJsonFile)
            {
                if (string.IsNullOrEmpty(_options.ResourcesPath))
                    throw new NotImplementedException("ResourcePath required with UseJsonFile.");

                services.AddTransient<IStringLocalizer, JsonLocalizer>();
            }

            return services;
        }


    }

    /// <summary>
    /// Ozellikler
    /// </summary>
    public class CustomLocalizationOptions : LocalizationOptions
    {
        public bool UseDatabase { get; set; }
        public bool UseJsonFile { get; set; }
    }


    /// <summary>
    /// Arakatman localizasyon uygulanıyor
    /// </summary>
    public class CustomLocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        // IMPORTANT NOTE: https://stackoverflow.com/questions/48590579/cannot-resolve-scoped-service-from-root-provider-net-core-2

        public CustomLocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private static readonly string[] StaticFileExtensions = { ".css", ".js", ".ico", ".woff", ".woff2", ".map", ".png", "gif", ".jpg", ".jpeg", ".svg" };

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string path = httpContext.Request.Path;

            if (StaticFileExtensions.Any(i => path.EndsWith(i, System.StringComparison.OrdinalIgnoreCase)))
            {
                await _next(httpContext);
                return;
            }





            await _next(httpContext);

            //Donen ortak obje
            var response = httpContext.Response;
        }
    }


}