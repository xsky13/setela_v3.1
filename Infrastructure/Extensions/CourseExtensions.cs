using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Infrastructure.Extensions
{
    public static class CourseExtensions
    {
        public static async Task<T?> LoadResources<T>(
            this IQueryable<T> query, 
            AppDbContext _db,
            IMapper _mapper,
            ResourceParentType parentType,
            CancellationToken cancellationToken = default) where T : class, IResourceable
        {
            var dto = await query.FirstOrDefaultAsync(cancellationToken);
            if (dto == null) return null;

            dto.Resources = await GetResourcesAsync(_db, _mapper, dto.Id, parentType, cancellationToken);

            if (dto is CourseDTO courseDto)
            {
                foreach (var topic in courseDto.TopicSeparators)
                {
                    topic.Resources = await GetResourcesAsync(
                        _db,
                        _mapper,
                        topic.Id,
                        ResourceParentType.TopicSeparator,
                        cancellationToken);
                }
            }

            return dto;
        }

        private static async Task<List<CourseResourceDTO>> GetResourcesAsync(
            AppDbContext db,
            IMapper mapper,
            int parentId,
            ResourceParentType type,
            CancellationToken ct)
                {
                    var entities = await db.Resources
                        .Include(r => r.SysUser)
                        .Where(r => r.ParentId == parentId && r.ParentType == type)
                        .ToListAsync(ct);

                    return mapper.Map<List<CourseResourceDTO>>(entities);
        }
    }
}
