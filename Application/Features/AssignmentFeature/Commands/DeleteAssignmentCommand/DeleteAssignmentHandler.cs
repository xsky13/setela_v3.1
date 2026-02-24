using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.DeleteAssignmentCommand
{
    public class DeleteAssignmentHandler(AppDbContext _db, IPermissionHandler _userPermissions, IResourceCleanupService _cleanupService) : IRequestHandler<DeleteAssignmentCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteAssignmentCommand command, CancellationToken cancellationToken)
        {
            var assignment = await _db.Assignments.FindAsync([command.AssignmentId], cancellationToken);
            if (assignment == null) return Result<object>.Fail("El trabajo practico no existe");

            if (!await _userPermissions.CanEditCourse(command.UserId, assignment.CourseId))
                return Result<object>.Fail("No puede modificar trabajos practicos en este curso", 403);

            var assignmentSubmissionIds = await _db.AssignmentSubmissions
                .Where(a => a.AssignmentId == assignment.Id)
                .Select(a => a.Id)
                .ToListAsync(cancellationToken);


            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await _cleanupService.ClearProgress(ProgressParentType.Assignment, assignment.Id);

                _db.Assignments.Remove(assignment);

                List<Resource> resourcesToDelete = [];

                var response = await _cleanupService.ClearMultipleResources(assignmentSubmissionIds, ResourceParentType.AssignmentSubmission, cancellationToken);
                resourcesToDelete.AddRange(response);

                var assignmentResponse = await _cleanupService.ClearParentResources(assignment.Id, ResourceParentType.Assignment, cancellationToken);
                resourcesToDelete.AddRange(assignmentResponse);

                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                await _cleanupService.ClearResourceFiles(resourcesToDelete);

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
