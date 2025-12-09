using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;
using System.Threading;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.RemoveProfessorCommand
{
    public class RemoveProfessorHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<RemoveProfessorCommand, Result<CourseDTO>>
    {
        public async Task<Result<CourseDTO>> Handle(RemoveProfessorCommand command, CancellationToken cancellationToken)
        {
            var professorToRemove = await _db.SysUsers
                .Include(user => user.ProfessorCourses)
                .FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);
            if (professorToRemove == null) return Result<CourseDTO>.Fail("Usuario no existe");

            var course = await _db.Courses
                .Include(course => course.Professors)
                .FirstOrDefaultAsync(course => course.Id == command.CourseId, cancellationToken);
            if (course == null) return Result<CourseDTO>.Fail("Curso no existe");

            if (!professorToRemove.ProfessorCourses.Any(course => course.Id == command.CourseId))
                return Result<CourseDTO>.Fail("El usuario no es profesor de este curso");

            professorToRemove.ProfessorCourses.Remove(course);
            course.Professors.Remove(professorToRemove);

            await _db.SaveChangesAsync(cancellationToken);

            return Result<CourseDTO>.Ok(_mapper.Map<CourseDTO>(course));
        }
    }
}
