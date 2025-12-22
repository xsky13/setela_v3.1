using AutoMapper;
using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.UpdateAssignmentCommand
{
    public class UpdateAssignmentHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<UpdateAssignmentCommand, Result<AssignmentDTO>>
    {
        public async Task<Result<AssignmentDTO>> Handle(UpdateAssignmentCommand command, CancellationToken cancellationToken)
        {
            if (!await _userPermissions.CanEditCourse(command.UserId, command.Assignment.CourseId))
                return Result<AssignmentDTO>.Fail("No puede modificar trabajos practicos en este curso", 403);

            var assignment = await _db.Assignments.FindAsync([command.AssignmentId], cancellationToken);
            if (assignment == null) return Result<AssignmentDTO>.Fail("El trabajo practico no existe");

            if (command.Assignment.Title != null) assignment.Title = command.Assignment.Title;
            if (command.Assignment.TextContent != null) assignment.TextContent = command.Assignment.TextContent;
            if (command.Assignment.MaxGrade.HasValue) assignment.MaxGrade = command.Assignment.MaxGrade.Value;
            if (command.Assignment.Weight.HasValue) assignment.Weight = command.Assignment.Weight.Value;
            if (command.Assignment.DueDate.HasValue) assignment.DueDate = command.Assignment.DueDate.Value;
            if (command.Assignment.Visible.HasValue) assignment.Visible = command.Assignment.Visible.Value;

            await _db.SaveChangesAsync(cancellationToken);
            return Result<AssignmentDTO>.Ok(_mapper.Map<AssignmentDTO>(assignment));
        }
    }
}
