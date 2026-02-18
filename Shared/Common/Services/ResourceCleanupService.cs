using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using System.Threading;

namespace SetelaServerV3._1.Shared.Common.Services
{
    public class ResourceCleanupService(AppDbContext _db, IFileStorage _storageService) : IResourceCleanupService
    {

        public async Task<List<Resource>> ClearMultipleResources(List<int> parentIds, ResourceParentType parentType, CancellationToken cancellationToken)
        {
            var resourcesToDelete = await _db.Resources
                .Where(r => r.ParentType == parentType && parentIds.Contains(r.ParentId))
                .ToListAsync(cancellationToken);

            if (resourcesToDelete.Count != 0)
                _db.Resources.RemoveRange(resourcesToDelete);

            return resourcesToDelete;
        }

        public async Task<List<Resource>> ClearParentResources(int parentId, ResourceParentType parentType, CancellationToken cancellationToken)
        {
            var resourcesToDelete = await _db.Resources
                .Where(r => r.ParentId == parentId && r.ParentType == parentType)
                .ToListAsync(cancellationToken);

            if (resourcesToDelete.Count != 0)
                _db.Resources.RemoveRange(resourcesToDelete);

            return resourcesToDelete;
        }

        public async Task ClearResourceFiles(List<Resource> resources)
        {
            foreach (var rd in resources)
            {
                if (rd.ResourceType == ResourceType.Document)
                {
                    var filename = Path.GetFileName(rd.Url);
                    await _storageService.DeleteFile(filename, rd.SysUserId);
                }
            }
        }
    }
}
