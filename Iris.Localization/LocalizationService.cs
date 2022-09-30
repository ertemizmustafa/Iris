using Iris.Localization.Abstractions;
using Iris.Localization.Abstractions.Data;
using Iris.Localization.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Localization
{
    internal class LocalizationService : ILocalizationService
    {
        private readonly LocalizationDbContext _localizationDbContext;

        public LocalizationService(LocalizationDbContext dbContext)
        {
            _localizationDbContext = dbContext;
        }

        public async Task<IEnumerable<ResourceData>> GetResourceDatas(CancellationToken cancellationToken)
        {
            return await _localizationDbContext.ResourceDatas.AsNoTracking().ToListAsync(cancellationToken);
            //.Where(x => x.GroupCode == groupCode && x.Code == code && x.IsActive && x.DefinitionGroup.IsActive)
        }
    }
}
