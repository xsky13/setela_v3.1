using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Commands.CreateGradeCommand
{
    public class CreateGradeHandler(AppDbContext _db, IMapper _mapper, IPermissionHandler _userPermissions) : IRequestHandler<CreateGradeCommand, Result<GradeDTO>>
    {
        public async Task<Result<GradeDTO>> Handle(CreateGradeCommand command, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(command.Grade.ParentType, true, out GradeParentType parentGradeType))
                return Result<GradeDTO>.Fail("Incorrect grade parent type");

            if (!await ParentExistsAsync(parentGradeType, command.Grade.ParentId, cancellationToken))
                return Result<GradeDTO>.Fail("La entidad no existe", 404);

            if (!await _userPermissions.CanEditCourse(command.UserId, command.Grade.CourseId))
                return Result<GradeDTO>.Fail("No tiene permisos para poner nota.", 403);

            var grade = new Grade
            { 
                Value = command.Grade.Value,
                ParentType = parentGradeType,
                ParentId = command.Grade.ParentId,
                SysUserId = command.Grade.StudentId
            };

            _db.Grades.Add(grade);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<GradeDTO>.Ok(_mapper.Map<GradeDTO>(grade));
        }

        private async Task<bool> ParentExistsAsync(GradeParentType parentType, int parentId, CancellationToken cancellationToken)
        {
            return parentType switch
            {
                GradeParentType.AssignmentSubmission => await _db.AssignmentSubmissions.AnyAsync(m => m.Id == parentId, cancellationToken),
                _ => false,
            };
        }
    }
}
