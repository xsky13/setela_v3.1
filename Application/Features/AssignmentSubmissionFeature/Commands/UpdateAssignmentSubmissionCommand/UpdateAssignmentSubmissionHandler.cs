using AutoMapper;
using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.UpdateAssignmentSubmissionCommand
{
    public class UpdateAssignmentSubmissionHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<UpdateAssignmentSubmissionCommand, Result<AssignmentSubmissionDTO>>
    {
        public async Task<Result<AssignmentSubmissionDTO>> Handle(UpdateAssignmentSubmissionCommand command, CancellationToken cancellationToken)
        {
            var assignmentSubmission = await _db.AssignmentSubmissions.FindAsync([command.AssignmentSubmissionId], cancellationToken);
            if (assignmentSubmission == null) return Result<AssignmentSubmissionDTO>.Fail("El trabajo practico no existe");

            if (command.UserId != assignmentSubmission.SysUserId)
                return Result<AssignmentSubmissionDTO>.Fail("No puede editar este trabajo practico");

            if (command.AssignmentSubmission.TextContent != null) assignmentSubmission.TextContent = command.AssignmentSubmission.TextContent;

            assignmentSubmission.LastUpdateDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);

            return Result<AssignmentSubmissionDTO>.Ok(_mapper.Map<AssignmentSubmissionDTO>(assignmentSubmission));
        }
    }
}
