using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.UpdateCourse
{
    public class UpdateCourseHandler(AppDbContext _db, IPermissionHandler _permissions) : IRequestHandler<UpdateCourseCommand, Result<Course>>
    {
        public async Task<Result<Course>> Handle(UpdateCourseCommand command, CancellationToken cancellationToken)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(course => course.Id == command.Id, cancellationToken);
            if (course == null) return Result<Course>.Fail("El curso no existe");

            var canEditCourse = await _permissions.CanEditCourse(command.UserId, course.Id);
            if (!canEditCourse) return Result<Course>.Fail("No autorizado", 403);

            course.Title = command.Title;
            course.Description = command.Description;
            course.IsActive = command.IsActive;

            await _db.Enrollments
                .Where(e => e.CourseId == course.Id && e.Valid != command.IsActive)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Valid, command.IsActive), cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            return Result<Course>.Ok(course);
        }
    }
}
