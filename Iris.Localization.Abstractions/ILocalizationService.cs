using Iris.Localization.Abstractions.Data;

namespace Iris.Localization.Abstractions
{
    public interface ILocalizationService
    {
        Task<IEnumerable<ResourceData>> GetResourceDatas(CancellationToken cancellationToken);
        Task<IEnumerable<ResourceData>> GetAllResourceDatas(CancellationToken cancellationToken);
        Task<int> CreateResourceData(CancellationToken cancellationToken);
        Task<bool> UpdateResourceData(CancellationToken cancellationToken);
        Task<bool> DeleteResourceData(CancellationToken cancellationToken);
        Task<bool> SyncronizeResourceChanges(CancellationToken cancellationToken);
    }
}

