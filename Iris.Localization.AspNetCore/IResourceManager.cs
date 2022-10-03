using System.Globalization;

namespace Iris.Localization.AspNetCore
{
    public interface IResourceManager
    {
        IEnumerable<string> GetAllStrings(bool includeParentCultures);

        IEnumerable<string> GetAllStrings(bool includeParentCultures, CultureInfo culture);

        string GetString(string name);

        string GetString(string name, CultureInfo culture);
    }
}
