using Iris.Localization.Abstractions.Data;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Iris.Localization.AspNetCore
{
    public class ResourceManager : IResourceManager
    {
        private readonly string _baseName;
        private readonly IrisLocalizationOptions _options;
        private readonly ILogger _logger;
        private readonly List<ResourceData> _resourceDatas;

        public ResourceManager(string baseName, IrisLocalizationOptions options, ILogger logger)
        {
            _baseName = baseName;
            _options = options;
            _logger = logger;
            _resourceDatas = new List<ResourceData>();
        }

        public IEnumerable<string> GetAllStrings(bool includeParentCultures)
        {
            return GetAllStrings(includeParentCultures, CultureInfo.CurrentUICulture);
        }

        public IEnumerable<string> GetAllStrings(bool includeParentCultures, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            InitializeData();

            if (includeParentCultures)
            {
                foreach (var s in _resourceDatas.Where(x => new CultureInfo(x.CultureName).Parent == new CultureInfo(culture.Name).Parent))
                {
                    yield return s.Name;
                }
            }
            else
            {
                foreach (var s in _resourceDatas.Where(x => x.CultureName == culture.Name))
                {
                    yield return s.Name;
                }
            }
        }

        public string GetString(string name)
        {
            return GetString(name, CultureInfo.CurrentUICulture);
        }

        public string GetString(string name, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            InitializeData();

            ResourceData? result = _resourceDatas?.FirstOrDefault(x => x.CultureName == culture.Name && x.Name == name);
            if (result != null) return result.Value;

            return _resourceDatas?.FirstOrDefault(x => string.IsNullOrEmpty(x.CultureName) && x.Name == name)?.Value ?? "";
        }

        private IEnumerable<ResourceData> InitializeData()
        {

            if (_resourceDatas.Any())
                yield return null;

            yield return new ResourceData { Id = 1, CultureName = "tr-TR", Name = "Star", Value = "Yıldız", Path = "" };
            yield return new ResourceData { Id = 2, CultureName = "tr-TR", Name = "Black", Value = "Siyah", Path = "" };
            yield return new ResourceData { Id = 2, CultureName = "tr-TR", Name = "ERROR_OCCURED", Value = "Bir hata oluştu.", Path = "" };
            yield return new ResourceData { Id = 2, CultureName = "en-US", Name = "ERROR_OCCURED", Value = "Opps.. Error occured while processing..", Path = "" };

        }
    }
}
