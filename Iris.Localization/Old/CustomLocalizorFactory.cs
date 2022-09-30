using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Localization.Old
{
    public class CustomLocalizorFactory : IStringLocalizerFactory
    {
        public CustomLocalizorFactory()
        {

        }

        public IStringLocalizer Create(Type resourceSource)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            throw new NotImplementedException();
        }
    }
}
