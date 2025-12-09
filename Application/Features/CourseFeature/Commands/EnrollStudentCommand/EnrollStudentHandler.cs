using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.EnrollStudentCommand
{
    public class EnrollStudentHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<EnrollStudentCommand, Result<CourseDTO>>
    {
        public async Task<Result<CourseDTO>> Handle(EnrollStudentCommand command, CancellationToken cancellationToken)
        {
            var user = await _db.SysUsers.Include(user => user.Enrollments).FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);
            if (user == null) return Result<CourseDTO>.Fail("El usuario no existe");

            var course = await _db.Courses.Include(user => user.Enrollments).FirstOrDefaultAsync(course => course.Id == command.CourseId, cancellationToken);
            if (course == null) return Result<CourseDTO>.Fail("El curso no existe");

            // check belonging
            if (user.Enrollments.Any(enrollment => enrollment.CourseId == command.CourseId))
                return Result<CourseDTO>.Fail("El usuario ya esta en el curso");

            var enrollment = new Enrollment
            {
                CourseId = command.CourseId,
                SysUserId = command.UserId,
                EnrollmentDate = DateTime.UtcNow,
                Valid = true,
            };

            user.Enrollments.Add(enrollment);
            course.Enrollments.Add(enrollment);

            await _db.SaveChangesAsync(cancellationToken);

            return Result<CourseDTO>.Ok(_mapper.Map<CourseDTO>(course));
        }
    }
}
