using Iris.Localization.AspNetCore.Old;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Localization.AspNetCore
{
    public class IrisStringLocalizer : IStringLocalizer
    {


        private readonly ConcurrentDictionary<string, object> _missingCache = new ConcurrentDictionary<string, object>();
        private readonly IResourceManager _resourceManager;
        private readonly string _baseName;
        private readonly ILogger _logger;

        public IrisStringLocalizer(IResourceManager resourceManager, string baseName, ILogger logger)
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

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }
    }
}
