using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.DeleteResourceCommand;
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

            var resourcesToDelete = await _db.Resources
                .Where(r => r.ParentId == assignment.Id && r.ParentType == Domain.Enums.ResourceParentType.Assignment)
                .ToListAsync(cancellationToken);

            await _cleanupService.ClearParentResources(assignment.Id, ResourceParentType.Assignment, cancellationToken);

            _db.Resources.RemoveRange(resourcesToDelete);
            _db.Assignments.Remove(assignment);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
