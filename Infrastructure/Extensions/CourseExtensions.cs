using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;

namespace SetelaServerV3._1.Infrastructure.Extensions
{
    public static class CourseExtensions
    {
        public static async Task<CourseDTO?> LoadResources(
            this IQueryable<CourseDTO> query, 
            AppDbContext _db,
            IMapper _mapper,
            CancellationToken cancellationToken = default)
        {
            var dto = await query.FirstOrDefaultAsync(cancellationToken);
            if (dto == null) return null;

            var resources = await _db.Resources
                .Include(r => r.SysUser)
                .Where(r => r.ParentId == dto.Id && r.ParentType == ResourceParentType.Course)
                .ToListAsync(cancellationToken);

            dto.Resources = _mapper.Map<List<CourseResourceDTO>>(resources);

            return dto;
        }
    }
}
