using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.DeleteAssignmentCommand
{
    public class DeleteAssignmentHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<DeleteAssignmentCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteAssignmentCommand command, CancellationToken cancellationToken)
        {
            var assignment = await _db.Assignments.FindAsync([command.AssignmentId], cancellationToken);
            if (assignment == null) return Result<object>.Fail("El trabajo practico no existe");

            if (!await _userPermissions.CanEditCourse(command.UserId, assignment.CourseId))
                return Result<object>.Fail("No puede modificar trabajos practicos en este curso", 403);

            _db.Assignments.Remove(assignment);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
