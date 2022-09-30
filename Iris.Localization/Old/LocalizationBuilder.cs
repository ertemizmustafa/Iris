using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Localization.Old
{
    internal class LocalizationBuilder
    {
        public static List<string> Languages = new();
        private readonly string _defaultLanguage = Constants.DefaultCulture.Name;
        private static readonly List<LocalizationResource> LocalizationResources = new();
        public static Dictionary<string, IConfiguration> LocalizationConfigurations = new();
        public static IConfiguration? DefaultLocalizationConfiguration;


        public string ResourcePath { get; private set; }

        public LocalizationBuilder()
        {

        }

        public LocalizationBuilder SetResourcePath(string path)
        {
            ResourcePath = path;
            return this;
        }

        public void Build()
        {
            BuildLocalizationResourcesIndex();

            BuildLanguageIndex();

            LocalizationConfigurations.Clear();

            foreach (var language in Languages)
            {
                var localizationConfiguration = BuildLocalizationResourceConfiguration(language);
                LocalizationConfigurations.Add(language, localizationConfiguration);

                if (language == _defaultLanguage)
                {
                    DefaultLocalizationConfiguration = localizationConfiguration;
                }
            }
        }

        private IConfiguration BuildLocalizationResourceConfiguration(string language)


        {
            var configurationBuilder = new ConfigurationBuilder();

            var languageResources = LocalizationResources.Where(l => l.Language == language).ToList();

            foreach (var resource in languageResources)
            {
                // stream'i using içine almayalım, configurationBuilder.Build() derken açık şekilde lazım oluyor, kendisi kapatıyor.
                var jsonStream = resource.Assembly.GetManifestResourceStream(resource.ResourceName);
                configurationBuilder.AddJsonStream(jsonStream);
            }

            return configurationBuilder.Build();
        }

        private void BuildLocalizationResourcesIndex()
        {
            LocalizationResources.Clear();

            foreach (var userAssembly in AssemblyHelper.domainAssemblies)
            {
                var resourceNames = GetAllLocalizationResourceNames(userAssembly);

                foreach (var resourceName in resourceNames)
                {
                    var localizationResourceInfo = new LocalizationResource
                    {
                        ResourceName = resourceName,
                        Assembly = userAssembly,
                        Language = ExtractLanguageFromLocalizationResourceFile(resourceName)
                    };

                    LocalizationResources.Add(localizationResourceInfo);
                }
            }
        }

        private string[] GetAllLocalizationResourceNames(Assembly assembly)
        {
            var resourceNames = assembly.GetManifestResourceNames();

            if (resourceNames.GetLength(0) == 0)
            {
                return resourceNames;
            }

            var localizationResourceNames = resourceNames.Where(r => r.EndsWithPattern("Resources..{5}.json")).ToArray();

            return localizationResourceNames;
        }


        private string ExtractLanguageFromLocalizationResourceFile(string resourceName)
        {
            var trimmed = resourceName.Replace(".json", "");
            var languageName = trimmed.Split('.').Last();
            return languageName;
        }

        private void BuildLanguageIndex()
        {
            Languages.Clear();
            Languages = LocalizationResources.Select(r => r.Language).Distinct().ToList();
        }
    }

    public enum SourceOption
    {
        JSONFILE,
        DATABASE,
    }

    internal class LocalizationResource
    {
        public IConfiguration Configuration { get; set; }
        public string ResourceName { get; set; }
        public Assembly Assembly { get; set; }
        public string Language { get; set; }
        public SourceOption Source { get; set; }
    }
}
