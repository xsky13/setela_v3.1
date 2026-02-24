using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.Queries.GetProgressQuery
{
    public class GetProgressHandler(AppDbContext _db) : IRequestHandler<GetProgressQuery, Result<int>>
    {
        public async Task<Result<int>> Handle(GetProgressQuery query, CancellationToken cancellationToken)
        {
            var totalItems = await _db.Modules.CountAsync(m => m.CourseId == query.CourseId, cancellationToken) +
                            await _db.Assignments.CountAsync(a => a.CourseId == query.CourseId, cancellationToken) +
                            await _db.Exams.CountAsync(e => e.CourseId == query.CourseId, cancellationToken) +
                            await _db.Resources.CountAsync(r => r.ParentType == ResourceParentType.Course && r.ParentId == query.CourseId, cancellationToken);

            if (totalItems == 0) return Result<int>.Ok(0);

            var progressItems = await _db.UserProgress
                .Where(p => p.CourseId == query.CourseId && p.SysUserId == query.UserId)
                .CountAsync(cancellationToken);

            var progress = (progressItems * 100) / totalItems;
            return Result<int>.Ok(progress);
        }
    }
}
