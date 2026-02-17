using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Shared.Common.Interfaces
{
    public interface IResourceCleanupService
    {
        Task ClearParentResources(int parentId, ResourceParentType parentType, CancellationToken ct);
    }
}
