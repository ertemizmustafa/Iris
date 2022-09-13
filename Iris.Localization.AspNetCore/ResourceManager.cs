using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Localization.AspNetCore
{
    internal class ResourceManager : IResourceManager
    {

        private readonly string _baseName;
        private readonly HcLocalizationOptions _hcLocalizationOptions;
        private readonly ILogger _logger;
        private readonly List<LocalData> _stringData;

        public ResourceManager(string baseName, HcLocalizationOptions hcLocalizationOptions, ILogger logger)
        {
            _baseName = baseName;
            _hcLocalizationOptions = hcLocalizationOptions;
            _logger = logger;
            _stringData = new List<LocalData>();
        }

        public IEnumerable<string> GetAllNames(bool includeParentCultures)
        {
            return GetAllNames(includeParentCultures, CultureInfo.CurrentUICulture);
        }

        public IEnumerable<string> GetAllNames(bool includeParentCultures, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            InitializeData();

            if (includeParentCultures)
            {
                foreach (var s in _stringData.Where(x => new CultureInfo(x.CultureName).Parent == new CultureInfo(culture.Name).Parent))
                {
                    yield return s.Name;
                }
            }
            else
            {
                foreach (var s in _stringData.Where(x => x.CultureName == culture.Name))
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

            LocalData result = _stringData.FirstOrDefault(x => x.CultureName == culture.Name && x.Name == name);
            if (result != null) return result.Value;

            return _stringData.FirstOrDefault(x => string.IsNullOrEmpty(x.CultureName) && x.Name == name)?.Value;
        }


        private void InitializeData()
        {
            if (_stringData.Any()) return;

        }
    }

    internal class LocalData
    {
        public string CultureName { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
