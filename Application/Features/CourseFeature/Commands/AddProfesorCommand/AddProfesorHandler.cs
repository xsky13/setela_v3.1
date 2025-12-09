using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.AddProfesorCommand
{
    public class AddProfesorHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<AddProfesorCommand, Result<CourseDTO>>
    {
        public async Task<Result<CourseDTO>> Handle(AddProfesorCommand command, CancellationToken cancellationToken)
        {
            var userToAdd = await _db.SysUsers
                .Include(user => user.ProfessorCourses)
                .FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);
            if (userToAdd == null) return Result<CourseDTO>.Fail("Usuario no existe");

            var course = await _db.Courses
                .Include(course => course.Professors)
                .FirstOrDefaultAsync(course => course.Id == command.CourseId, cancellationToken);
            if (course == null) return Result<CourseDTO>.Fail("Curso no existe");

            if (userToAdd.ProfessorCourses.Any(course => course.Id == command.CourseId))
                return Result<CourseDTO>.Fail("El usuario ya es profesor de este curso");

            userToAdd.ProfessorCourses.Add(course);
            course.Professors.Add(userToAdd);

            await _db.SaveChangesAsync(cancellationToken);

            return Result<CourseDTO>.Ok(_mapper.Map<CourseDTO>(course));
        }
    }
}
