using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Infrastructure.Extensions;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Queries.GetAssignmentByIdQuery
{
    public class GetAssignmentByIdHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetAssignmentByIdQuery, Result<AssignmentDTO>>
    {
        public async Task<Result<AssignmentDTO>> Handle(GetAssignmentByIdQuery query, CancellationToken cancellationToken)
        {
            var assignment = await _db.Assignments
                .Where(a => a.Id == query.AssignmentId)
                .ProjectTo<AssignmentDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                .LoadResources(_db, _mapper, Domain.Enums.ResourceParentType.Assignment, cancellationToken);

            if (assignment == null) return Result<AssignmentDTO>.Fail("El trabajo practico no existe", 404);

            var submissionIds = assignment.AssignmentSubmissions.Select(s => s.Id).ToList();
            var gradesDict = await _db.Grades
                .Where(g => g.ParentType == Domain.Enums.GradeParentType.AssignmentSubmission &&
                            submissionIds.Contains(g.ParentId))
                .ToDictionaryAsync(g => g.ParentId, g => g.Value, cancellationToken);

            foreach (var submission in assignment.AssignmentSubmissions)
            {
                if (gradesDict.TryGetValue(submission.Id, out var gradeValue))
                {
                    submission.Grade = gradeValue;
                }
            }

            if (assignment == null) return Result<AssignmentDTO>.Fail("El trabajo practico no existe", 404);

            return Result<AssignmentDTO>.Ok(assignment);
        }
    }
}
