using MediatR;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Commands.DeleteGradeCommand
{
    public class DeleteGradeHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<DeleteGradeCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(DeleteGradeCommand command, CancellationToken cancellationToken)
        {
            var grade = await _db.Grades.FindAsync([command.GradeId], cancellationToken);
            if (grade == null) return Result<object>.Fail("La nota no existe.");

            if(!await _userPermissions.CanEditCourse(command.UserId, grade.CourseId))
                return Result<object>.Fail("No tiene permisos para cambiar esta nota.", 403);

            _db.Grades.Remove(grade);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<object>.Ok(new { Success = true });
        }
    }
}
