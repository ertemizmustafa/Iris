using Microsoft.Extensions.Localization;

namespace Iris.Localization.AspNetCore
{
    /// <summary>
    /// Ozellikler
    /// </summary>
    public class HcLocalizationOptions : LocalizationOptions
    {
        public LocalizationSourceType SourceType { get; set; }
        public string? ConnectionString { get; set; }
    }
}