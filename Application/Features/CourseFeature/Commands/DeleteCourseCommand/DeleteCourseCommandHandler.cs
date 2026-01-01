using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.DeleteCourseCommand
{
    public class DeleteCourseCommandHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<DeleteCourseCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteCourseCommand command, CancellationToken cancellationToken)
        {
            var canDelete = await _userPermissions.CanEditCourse(command.UserId, command.CourseId);
            if (!canDelete) return Result<object>.Fail("No tiene permisos para eliminar este curso", 403);

            var course = await _db.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == command.CourseId, cancellationToken);

            if (course == null) return Result<object>.Fail("El curso no existe", 404);

            foreach (var enrollment in course.Enrollments)
                enrollment.Valid = false;

            //_db.Courses.Remove(course);
            course.IsActive = false;

            await _db.SaveChangesAsync(cancellationToken);
            return Result<object>.Ok(new { Success = true });
        }
    }
}
