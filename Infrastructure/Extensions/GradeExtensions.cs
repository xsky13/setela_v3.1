using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Infrastructure.Extensions
{
    public static class GradeExtensions
    {
        public static async Task<T?> LoadGrades<T>(
            this IQueryable<T> query,
            AppDbContext _db,
            IMapper _mapper,
            GradeParentType parentType,
            CancellationToken cancellationToken = default) where T : class, IGradeable
        {
            var dto = await query.FirstOrDefaultAsync(cancellationToken);
            if (dto == null) return null;

            dto.Grade = await _db.Grades
                .Where(r => r.ParentId == dto.Id && r.ParentType == parentType)
                .ProjectTo<GradeSimpleDTO>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);

            return dto;
        }
    }
}
