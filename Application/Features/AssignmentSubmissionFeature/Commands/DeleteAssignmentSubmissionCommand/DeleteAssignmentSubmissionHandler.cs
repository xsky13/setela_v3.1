using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.DeleteAssignmentSubmissionCommand
{
    public class DeleteAssignmentSubmissionHandler(AppDbContext _db, IPermissionHandler _userPermissions, IResourceCleanupService _cleanupService) : IRequestHandler<DeleteAssignmentSubmissionCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteAssignmentSubmissionCommand command, CancellationToken cancellationToken)
        {
            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var assignmentSubmission = await _db.AssignmentSubmissions
                    .Include(a => a.Assignment)
                    .FirstOrDefaultAsync(a => a.Id == command.AssignmentSubmissionId, cancellationToken);
                if (assignmentSubmission == null) return Result<object>.Fail("No existe la entrega.", 404);


                if (!await _userPermissions.CanModifyAssignmentSubmission(command.UserId, assignmentSubmission.SysUserId, assignmentSubmission.Assignment.CourseId))
                    return Result<object>.Fail("No tiene permisos para modificar esta entrega.", 503);

                _db.AssignmentSubmissions.Remove(assignmentSubmission);

                await _cleanupService.ClearParentResources(assignmentSubmission.Id, ResourceParentType.AssignmentSubmission, cancellationToken);

                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Result<object>.Ok(new { Success = true });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result<object>.Fail("Error al eliminar la entrega y sus archivos.");
            }
        }
    }
}
