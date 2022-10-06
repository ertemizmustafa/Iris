using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Localization.AspNetCore
{
    public class IrisLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IResourceNamesCache _resourceNamesCache = new ResourceNamesCache();
        private readonly ConcurrentDictionary<string, IrisStringLocalizer> _localizerCache = new ConcurrentDictionary<string, IrisStringLocalizer>();
        private readonly IrisLocalizationOptions _options;
        private readonly ILoggerFactory _loggerFactory;

        public IrisLocalizerFactory(IOptions<IrisLocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
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
            throw new NotImplementedException();
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

                return CreateIrisStringLocalizer(assembly, baseName);
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

        private static string TrimPrefix(string name, string prefix)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return name.Substring(prefix.Length);
            }

            return name;
        }

        protected virtual IrisStringLocalizer CreateIrisStringLocalizer(Assembly assembly, string baseName)
        {
            return new IrisStringLocalizer(new ResourceManager(baseName, _options, _loggerFactory.CreateLogger<IResourceManager>()), baseName, _loggerFactory.CreateLogger<IrisStringLocalizer>());
        }
    }
}
