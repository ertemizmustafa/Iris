using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Reflection;

namespace Iris.Localization.AspNetCore.Old
{
    public class HcLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IResourceNamesCache _resourceNamesCache = new ResourceNamesCache();
        private readonly ConcurrentDictionary<string, HcStringLocalizer> _localizerCache = new ConcurrentDictionary<string, HcStringLocalizer>();
        private readonly HcLocalizationOptions _options;
        private readonly ILoggerFactory _loggerFactory;

        public HcLocalizerFactory(IOptions<HcLocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
        {
            if (localizationOptions == null)
            {
                throw new ArgumentNullException(nameof(localizationOptions));
            }


            _options = localizationOptions.Value;
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        }

        public IStringLocalizer Create(Type resourceSource)
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException(nameof(resourceSource));
            }

            var typeInfo = resourceSource.GetTypeInfo();
            var assembly = typeInfo.Assembly;
            var assemblyName = new AssemblyName(assembly.FullName);

            var baseName = typeInfo.FullName ?? "";


            var resourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            return _localizerCache.GetOrAdd(baseName, _ => CreateHcStringLocalizer(assembly, baseName));
        }

        public IStringLocalizer Create(string baseName, string location)
        {

            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            return _localizerCache.GetOrAdd($"B={baseName},L={location}", _ =>
            {
                var assemblyName = new AssemblyName(location);
                var assembly = Assembly.Load(assemblyName);
                baseName = GetResourcePrefix(baseName, location);

                return CreateHcStringLocalizer(assembly, baseName);
            });
        }

        protected virtual string GetResourcePrefix(string baseResourceName, string baseNamespace)
        {
            if (string.IsNullOrEmpty(baseResourceName))
            {
                throw new ArgumentNullException(nameof(baseResourceName));
            }

            if (string.IsNullOrEmpty(baseNamespace))
            {
                throw new ArgumentNullException(nameof(baseNamespace));
            }

            var assemblyName = new AssemblyName(baseNamespace);
            var assembly = Assembly.Load(assemblyName);
            var locationPath = baseNamespace;

            baseResourceName = locationPath + TrimPrefix(baseResourceName, baseNamespace + ".");

            return baseResourceName;
        }

        protected virtual HcStringLocalizer CreateHcStringLocalizer(Assembly assembly, string baseName)
        {
            return new HcStringLocalizer(new ResourceManager(baseName, _options, _loggerFactory.CreateLogger<IResourceManager>()), baseName, _loggerFactory.CreateLogger<HcStringLocalizer>());
        }

        private static string TrimPrefix(string name, string prefix)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return name.Substring(prefix.Length);
            }

            return name;
        }
    }
}