using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Shared.Common.Interfaces
{
    public interface IResourceCleanupService
    {
        Task<List<Resource>> ClearParentResources(int parentId, ResourceParentType parentType, CancellationToken ct);
        Task<List<Resource>> ClearMultipleResources(List<int> parentIds, ResourceParentType parentType, CancellationToken ct);
        Task ClearResourceFiles(List<Resource> resources);
    }
}
