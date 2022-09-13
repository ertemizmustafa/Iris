using Microsoft.Extensions.Localization;

namespace Iris.Localization
{
    public class Localizor : IStringLocalizer<string>
    {

        private string Translate(string name)
        {
            string regulatedName = name.Replace(".", ":");

            //var translated = CurrentLocalizationConfiguration.GetValue<string>(regulatedName);

            //if (string.IsNullOrEmpty(translated) && CurrentLocalizationConfiguration != ResourceConfigurationBuilder.DefaultLocalizationConfiguration)
            //{
            //    translated = ResourceConfigurationBuilder.DefaultLocalizationConfiguration.GetValue<string>(regulatedName);
            //}

            //return translated;
            return "";
        }

        public LocalizedString this[string name]
        {
            get
            {
                var translated = Translate(name);

                if (string.IsNullOrEmpty(translated))
                {
                    translated = string.Empty;
                }

                return new LocalizedString(name, translated);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var translated = Translate(name);

                var resourceNotFound = false;

                if (string.IsNullO980rEmpty(translated))
                {
                    resourceNotFound = true;
                    translated = string.Empty;
                }
                else
                {
                    if (arguments != null)
                    {
                        translated = string.Format(translated, arguments);
                    }
                }

                return new LocalizedString(name, translated, resourceNotFound);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            //var allItems = CurrentLocalizationConfiguration.AsEnumerable();

            //var allStrings = new List<LocalizedString>();

            //foreach (var item in allItems)
            //{
            //    if (item.Value != null)
            //    {
            //        allStrings.Add(new LocalizedString(item.Key, item.Value));
            //    }
            //}

            //return allStrings;
            return default;
        }
    }
}
