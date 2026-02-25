using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Queries.GetGradesForStudentQuery
{
    public class GetGradesForStudentHandler(AppDbContext _db) : IRequestHandler<GetGradesForStudentQuery, Result<List<GradeDTO>>>
    {
        public async Task<Result<List<GradeDTO>>> Handle(GetGradesForStudentQuery query, CancellationToken cancellationToken)
        {
            var gradeList = await _db.Grades
                .Where(g => g.CourseId == query.CourseId && g.SysUserId == query.UserId)
                .Select(g => new GradeDTO
                {
                    Id = g.Id,
                    Value = g.Value,
                    ParentType = g.ParentType,
                    ParentId = g.ParentId,
                    CourseId = g.CourseId,
                    ParentHelper = g.ParentType == GradeParentType.AssignmentSubmission
                        ? _db.AssignmentSubmissions
                            .Where(s => s.Id == g.ParentId)
                            .Select(s => new ParentHelper
                            {
                                ItemTitle = s.Assignment.Title,
                                MaxGrade = s.Assignment.MaxGrade,
                                GrandParentId = s.AssignmentId
                            }).FirstOrDefault()
                        : _db.ExamSubmissions
                            .Where(s => s.Id == g.ParentId)
                            .Select(s => new ParentHelper
                            {
                                ItemTitle = s.Exam.Title,
                                MaxGrade = s.Exam.MaxGrade,
                                GrandParentId = s.ExamId
                            }).FirstOrDefault()
                })
                .ToListAsync(cancellationToken);


            return Result<List<GradeDTO>>.Ok(gradeList);
        }
    }
}
