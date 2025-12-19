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
    public static class ResourceExtensions
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

            dto.Resources = await _db.Resources
                .Where(r => r.ParentId == dto.Id && r.ParentType == parentType)
                .ProjectTo<CourseResourceDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return dto;
        }
    }
}
