using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Common.Interfaces;
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

            var parent = await ParentExistsAsync(parentGradeType, command.Grade.ParentId, cancellationToken);
            if (parent == null)
                return Result<GradeDTO>.Fail("La entidad no existe", 404);

            if (!await _userPermissions.CanEditCourse(command.UserId, command.Grade.CourseId))
                return Result<GradeDTO>.Fail("No tiene permisos para poner nota.", 403);

            // check if already exists
            if (parent.Grade != null)
                return Result<GradeDTO>.Fail("Ya hay nota.");

            var grade = new Grade
            { 
                Value = command.Grade.Value,
                ParentType = parentGradeType,
                ParentId = command.Grade.ParentId,
                SysUserId = command.Grade.StudentId,
                CourseId = command.Grade.CourseId
            };

            _db.Grades.Add(grade);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<GradeDTO>.Ok(_mapper.Map<GradeDTO>(grade));
        }

        private async Task<IGradeable?> ParentExistsAsync(GradeParentType parentType, int parentId, CancellationToken cancellationToken)
        {
            return parentType switch
            {
                GradeParentType.AssignmentSubmission => await _db.AssignmentSubmissions
                    .ProjectTo<AssignmentSubmissionDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(m => m.Id == parentId, cancellationToken),
                GradeParentType.ExamSubmission => await _db.ExamSubmissions
                    .ProjectTo<ExamSubmissionDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(m => m.Id == parentId, cancellationToken),
                _ => null,
            };
        }
    }
}
