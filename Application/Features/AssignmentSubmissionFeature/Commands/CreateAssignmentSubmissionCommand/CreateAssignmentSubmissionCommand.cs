using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.CreateAssignmentSubmissionCommand
{
    public class CreateAssignmentSubmissionCommand : IRequest<Result<AssignmentSubmissionDTO>>
    {
        public int UserId { get; set; }
        public CreateAssignmentSubmissionRequestDTO AssignmentSubmission { get; set; }
        public string BaseUrl { get; set; }
    }
}
