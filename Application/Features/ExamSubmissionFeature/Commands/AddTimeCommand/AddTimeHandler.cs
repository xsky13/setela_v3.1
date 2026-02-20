using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.AddTimeCommand
{
    public class AddTimeHandler(AppDbContext _db, IPermissionHandler _userPermissions, IMapper _mapper) : IRequestHandler<AddTimeCommand, Result<ExamSubmissionDTO>>
    {
        public async Task<Result<ExamSubmissionDTO>> Handle(AddTimeCommand command, CancellationToken cancellationToken)
        {
            var examSubmission = await _db.ExamSubmissions.FindAsync([command.ExamSubmissionId], cancellationToken);
            if (examSubmission == null) return Result<ExamSubmissionDTO>.Fail("No existe la entrega.");

            bool canEditCourse = await _userPermissions.CanEditCourse(command.UserId, command.CourseId);
            if (!canEditCourse) return Result<ExamSubmissionDTO>.Fail("No puede modificar este curso.", 403);

            examSubmission.Finished = false;
            examSubmission.AdminExtendedTime = true;

            await _db.SaveChangesAsync(cancellationToken);
            return Result<ExamSubmissionDTO>.Ok(_mapper.Map<ExamSubmissionDTO>(examSubmission));
        }
    }
}
