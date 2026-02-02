using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.DeleteAssignmentSubmissionCommand
{
    public class DeleteAssignmentSubmissionHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<DeleteAssignmentSubmissionCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteAssignmentSubmissionCommand command, CancellationToken cancellationToken)
        {
            var assignmentSubmission = await _db.AssignmentSubmissions
                .Include(a => a.Assignment)
                .FirstOrDefaultAsync(a => a.Id == command.AssignmentSubmissionId, cancellationToken);
            if (assignmentSubmission == null) return Result<object>.Fail("No existe la entrega.", 404);


            if (!await _userPermissions.CanModifyAssignmentSubmission(command.UserId, assignmentSubmission.SysUserId, assignmentSubmission.Assignment.CourseId))
                return Result<object>.Fail("No tiene permisos para modificar esta entrega.", 503);

            _db.AssignmentSubmissions.Remove(assignmentSubmission);

            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
