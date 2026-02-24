using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Queries.GetGradesForStudentQuery
{
    public class GetGradesForStudentHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetGradesForStudentQuery, Result<List<GradeDTO>>>
    {
        public async Task<Result<List<GradeDTO>>> Handle(GetGradesForStudentQuery query, CancellationToken cancellationToken)
        {
            var gradeList = await _db.Grades
                .ProjectTo<GradeDTO>(_mapper.ConfigurationProvider)
                .Where(g => g.CourseId == query.CourseId && g.SysUserId == query.UserId)
                .ToListAsync(cancellationToken);

            return Result<List<GradeDTO>>.Ok(gradeList);
        }
    }
}
