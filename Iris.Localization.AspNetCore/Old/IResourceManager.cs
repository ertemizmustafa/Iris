using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Localization.AspNetCore.Old
{
    public interface IResourceManager
    {
        IEnumerable<string> GetAllNames(bool includeParentCultures);

        IEnumerable<string> GetAllNames(bool includeParentCultures, CultureInfo culture);

        string GetString(string name);

        string GetString(string name, CultureInfo culture);
    }
}
