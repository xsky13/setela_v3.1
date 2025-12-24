using AutoMapper;
using MediatR;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Commands.UpdateGradeCommand
{
    public class UpdateGradeHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<UpdateGradeCommand, Result<GradeDTO>>
    {
        public async Task<Result<GradeDTO>> Handle(UpdateGradeCommand command, CancellationToken cancellationToken)
        {
            var grade = await _db.Grades.FindAsync([command.GradeId], cancellationToken);
            if (grade == null) return Result<GradeDTO>.Fail("La nota no existe.");

            if (!await _userPermissions.CanEditCourse(command.UserId, grade.CourseId))
                return Result<GradeDTO>.Fail("No tiene permisos para poner nota.", 403);

            grade.Value = command.Grade.Value;

            await _db.SaveChangesAsync(cancellationToken);
            return Result<GradeDTO>.Ok(_mapper.Map<GradeDTO>(grade));
        }
    }
}
