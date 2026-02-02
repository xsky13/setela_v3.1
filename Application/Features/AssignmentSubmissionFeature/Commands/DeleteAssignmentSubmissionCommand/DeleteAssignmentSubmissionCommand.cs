using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Commands.DeleteAssignmentSubmissionCommand
{
    public class DeleteAssignmentSubmissionCommand: IRequest<Result<object>>
    {
        public int UserId { get; set; }
        public int AssignmentSubmissionId { get; set; }
    }
}
