using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.Commands.FinishExamCommand
{
    public class FinishExamHandler(AppDbContext _db, IPermissionHandler _userPermissions, IMapper _mapper) : IRequestHandler<FinishExamCommand, Result<ExamSubmissionDTO>>
    {
        public async Task<Result<ExamSubmissionDTO>> Handle(FinishExamCommand command, CancellationToken cancellationToken)
        {
            // check if exam submission belongs to user
            var examSubmission = await _db.ExamSubmissions
                .Where(e => e.Id == command.ExamSubmissionId)
                .Select(e => new {
                    Submission = e,
                    e.Exam.CourseId
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (examSubmission == null) return Result<ExamSubmissionDTO>.Fail("La entrega no existe.");

            bool canEdit = await _userPermissions.CanEditCourse(command.UserId, examSubmission.CourseId);
            if (examSubmission.Submission.SysUserId != command.UserId && !canEdit)
                return Result<ExamSubmissionDTO>.Fail("No es dueño de la entrega.");

            // check if user already submitted
            if (examSubmission.Submission.Finished)
                return Result<ExamSubmissionDTO>.Fail("Esta entrega ya esta finalizada.");

            // change state
            examSubmission.Submission.Finished = true;
            await _db.SaveChangesAsync(cancellationToken);

            // return
            return Result<ExamSubmissionDTO>.Ok(_mapper.Map<ExamSubmissionDTO>(examSubmission));
        }
    }
}
