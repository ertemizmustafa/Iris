using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Globalization;

namespace Iris.Localization.AspNetCore
{
    public class HcStringLocalizer : IStringLocalizer
    {
        private readonly ConcurrentDictionary<string, object> _missingCache = new ConcurrentDictionary<string, object>();
        private readonly IResourceManager _resourceManager;
        private readonly string _baseName;
        private readonly ILogger _logger;

        public HcStringLocalizer(IResourceManager resourceManager, string baseName, ILogger logger)
        {
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            _baseName = baseName ?? throw new ArgumentNullException(nameof(baseName));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public LocalizedString this[string name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));

                var value = GetStringSafely(name, null);

                return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: _baseName);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));

                var format = GetStringSafely(name, null);
                var value = string.Format(format ?? name, arguments);

                return new LocalizedString(name, value, resourceNotFound: format == null, searchedLocation: _baseName);
            }
        }


        public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => GetAllStrings(includeParentCultures, CultureInfo.CurrentUICulture);

        private IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            foreach (var name in _resourceManager.GetAllNames(includeParentCultures, culture))
            {
                var value = GetStringSafely(name, culture);
                yield return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: _baseName);
            }
        }


        private string GetStringSafely(string name, CultureInfo culture)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var keyCulture = culture ?? CultureInfo.CurrentUICulture;

            var cacheKey = $"name={name}&culture={keyCulture.Name}";

            _logger.LogDebug($"{nameof(ResourceManagerStringLocalizer)} searched for '{name}' in '{_baseName}' with culture '{keyCulture}'.");

            if (_missingCache.ContainsKey(cacheKey)) return null;

            try
            {
                return culture == null ? _resourceManager.GetString(name) : _resourceManager.GetString(name, culture);
            }
            catch (Exception)
            {
                _missingCache.TryAdd(cacheKey, null);
                return null;
            }
        }
    }


}