using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.CreateAssignmentSubmissionCommand
{
    public class CreateAssignmentSubmissionHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<CreateAssignmentSubmissionCommand, Result<AssignmentSubmissionDTO>>
    {
        public async Task<Result<AssignmentSubmissionDTO>> Handle(CreateAssignmentSubmissionCommand command, CancellationToken cancellationToken)
        {
            var assignment = await _db.Assignments
                .Where(a => a.Id == command.AssignmentSubmission.AssignmentId)
                .Select(a => new { a.DueDate })
                .FirstOrDefaultAsync(cancellationToken);
            if (assignment == null) return Result<AssignmentSubmissionDTO>.Fail("El trabajo practico no existe.");

            //var dueDate = assignment.DueDate.Kind == DateTimeKind.Unspecified
            //    ? DateTime.SpecifyKind(assignment.DueDate, DateTimeKind.Utc)
            //    : assignment.DueDate.ToUniversalTime();

            //if (DateTime.UtcNow > dueDate)
            //    return Result<AssignmentSubmissionDTO>.Fail("La fecha límite ha pasado.");

            bool tareaExiste = await _db.AssignmentSubmissions
                .AnyAsync(a => a.SysUserId == command.UserId && 
                a.AssignmentId == command.AssignmentSubmission.AssignmentId, cancellationToken);
            if (tareaExiste)
                return Result<AssignmentSubmissionDTO>.Fail("Ya has enviado este trabajo.");

            var assignmentSubmission = new AssignmentSubmission
            {
                TextContent = command.AssignmentSubmission.TextContent,
                CreationDate = DateTime.UtcNow,
                SysUserId = command.UserId,
                AssignmentId = command.AssignmentSubmission.AssignmentId
            };

            _db.AssignmentSubmissions.Add(assignmentSubmission);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<AssignmentSubmissionDTO>.Ok(_mapper.Map<AssignmentSubmissionDTO>(assignmentSubmission));
        }
    }
}
