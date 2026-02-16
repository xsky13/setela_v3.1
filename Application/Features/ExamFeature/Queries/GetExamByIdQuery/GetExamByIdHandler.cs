using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ExamFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Infrastructure.Extensions;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamFeature.Queries.GetExamByIdQuery
{
    public class GetExamByIdHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetExamByIdQuery, Result<ExamDTO>>
    {
        public async Task<Result<ExamDTO>> Handle(GetExamByIdQuery query, CancellationToken cancellationToken)
        {
            var exam = await _db.Exams
                .Where(e => e.Id == query.ExamId)
                .ProjectTo<ExamDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                .LoadResources(_db, _mapper, Domain.Enums.ResourceParentType.Exam, cancellationToken);

            if (exam == null)
                return Result<ExamDTO>.Fail("El examen no existe.", 404);

            var submissionIds = exam.ExamSubmissions.Select(s => s.Id).ToList();
            var gradesDict = await _db.Grades
                .Where(g => g.ParentType == Domain.Enums.GradeParentType.ExamSubmission &&
                            submissionIds.Contains(g.ParentId))
                .ToDictionaryAsync(g => g.ParentId, g => g, cancellationToken);

            foreach (var submission in exam.ExamSubmissions)
            {
                if (gradesDict.TryGetValue(submission.Id, out var grade))
                {
                    submission.Grade = new Shared.Common.DTO.GradeSimpleDTO
                    {
                        Id = grade.Id,
                        Value = grade.Value,
                        SysUserId = grade.SysUserId
                    };
                }
            }

            return Result<ExamDTO>.Ok(exam);
        }
    }
}
