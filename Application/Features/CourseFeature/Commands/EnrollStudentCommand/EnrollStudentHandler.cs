using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.EnrollStudentCommand
{
    public class EnrollStudentHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<EnrollStudentCommand, Result<CourseDTO>>
    {
        public async Task<Result<CourseDTO>> Handle(EnrollStudentCommand command, CancellationToken cancellationToken)
        {
            var currentUser = await _db.SysUsers.FirstOrDefaultAsync(user => user.Id == command.CurrentUserId, cancellationToken);
            if (currentUser == null) return Result<CourseDTO>.Fail("El usuario no existe");
            // check permissions
            if (!_userPermissions.CanChangeStudents(currentUser, command.UserId, command.CourseId)) 
                return Result<CourseDTO>.Fail("No puede editar este curso", 403);

            var course = await _db.Courses
                .Include(course => course.Enrollments.Where(e => e.SysUserId == command.UserId))
                .FirstOrDefaultAsync(course => course.Id == command.CourseId, cancellationToken);
            if (course == null) return Result<CourseDTO>.Fail("El curso no existe");

            // check belonging
            if (course.Enrollments.Count != 0)
                return Result<CourseDTO>.Fail("El usuario ya esta en el curso");

            var enrollment = new Enrollment
            {
                CourseId = command.CourseId,
                SysUserId = command.UserId,
                EnrollmentDate = DateTime.UtcNow,
                Valid = true,
            };

            course.Enrollments.Add(enrollment);

            await _db.SaveChangesAsync(cancellationToken);

            return Result<CourseDTO>.Ok(_mapper.Map<CourseDTO>(course));
        }
    }
}
