using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.UpdateAssignmentSubmissionCommand
{
    public class UpdateAssignmentSubmissionCommand : IRequest<Result<AssignmentSubmissionDTO>>
    {
        public int UserId { get; set; }
        public int AssignmentSubmissionId { get; set; }
        public UpdateAssignmentSubmissionRequestDTO AssignmentSubmission { get; set; }
    }
}
