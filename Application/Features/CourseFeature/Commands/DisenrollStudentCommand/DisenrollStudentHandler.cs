using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.DisenrollStudentCommand
{
    public class DisenrollStudentHandler(AppDbContext _db, IPermissionHandler _userPermissions, IMapper _mapper) : IRequestHandler<DisenrollStudentCommand, Result<CourseDTO>>
    {
        public async Task<Result<CourseDTO>> Handle(DisenrollStudentCommand command, CancellationToken cancellationToken)
        {
            var currentUser = await _db.SysUsers.FirstOrDefaultAsync(user => user.Id == command.CurrentUserId, cancellationToken);
            if (currentUser == null) return Result<CourseDTO>.Fail("Usuario actual no existe");
            if (!_userPermissions.CanChangeStudents(currentUser, command.UserId, command.CourseId))
                return Result<CourseDTO>.Fail("No puede editar el curso", 403);

            //var enrollment = await _db.Enrollments
            //    .Include(enrollment => enrollment.Course)
            //    .FirstOrDefaultAsync(enrollment => 
            //        enrollment.SysUserId == command.UserId 
            //        && enrollment.CourseId == command.CourseId
            //        && enrollment.Valid, 
            //    cancellationToken);
            var enrollment = await _db.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == command.CourseId && e.SysUserId == command.UserId, cancellationToken);
            if (enrollment == null) return Result<CourseDTO>.Fail("El usuario no esta inscripto en el curso");

            //enrollment.Valid = false;
            _db.Enrollments.Remove(enrollment);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<CourseDTO>.Ok(_mapper.Map<CourseDTO>(enrollment.Course));
        }
    }
}
