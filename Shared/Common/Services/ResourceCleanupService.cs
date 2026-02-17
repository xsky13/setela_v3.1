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
        public async Task ClearParentResources(int parentId, ResourceParentType parentType, CancellationToken cancellationToken)
        {
            var resourcesToDelete = await _db.Resources
                .Where(r => r.ParentId == parentId && r.ParentType == parentType)
                .ToListAsync(cancellationToken);

            foreach (var rd in resourcesToDelete)
            {
                if (rd.ResourceType == ResourceType.Document)
                {
                    await _storageService.DeleteFile(rd.Url);
                }
            }

            if (resourcesToDelete.Count != 0)
            {
                _db.Resources.RemoveRange(resourcesToDelete);
            }
        }
    }
}
