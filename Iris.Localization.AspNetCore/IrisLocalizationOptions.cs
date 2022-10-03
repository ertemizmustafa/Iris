using Microsoft.Extensions.Localization;

namespace Iris.Localization.AspNetCore
{
    public class IrisLocalizationOptions : LocalizationOptions
    {
        public LocalizationSourceType SourceType { get; set; }
        public string? ConnectionString { get; set; }
    }

    public enum LocalizationSourceType
    {
        Database,
        Json,
        All
    }
}
