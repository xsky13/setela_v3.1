using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Services;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.CreateAssignmentCommand
{
    public class CreateAssignmentHandler(AppDbContext _db, IPermissionHandler _userPermissions, MaxDisplayOrder maxDisplayOrder) : IRequestHandler<CreateAssignmentCommand, Result<Assignment>>
    {
        public async Task<Result<Assignment>> Handle(CreateAssignmentCommand command, CancellationToken cancellationToken)
        {
            if (!await _userPermissions.CanEditCourse(command.UserId, command.Assignment.CourseId))
                return Result<Assignment>.Fail("No puede agregar trabajos practicos a este curso", 403);

            var courseExists = await _db.Courses.AnyAsync(course => course.Id == command.Assignment.CourseId, cancellationToken);
            if (!courseExists) return Result<Assignment>.Fail("El curso que especifico no existe");

            var assignment = new Assignment
            { 
                Title = command.Assignment.Title,
                TextContent = command.Assignment.TextContent,
                DueDate = command.Assignment.DueDate,
                CreationDate = DateTime.UtcNow,
                MaxGrade = command.Assignment.MaxGrade,
                Weight = command.Assignment.Weight,
                Visible = false,
                DisplayOrder = await maxDisplayOrder.GetNext(command.Assignment.CourseId, cancellationToken),
                CourseId = command.Assignment.CourseId
            };

            _db.Assignments.Add(assignment);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<Assignment>.Ok(assignment);
        }
    }
}
