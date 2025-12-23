using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.Queries.GetAssignmentSubmissionByIdQuery
{
    public class GetAssignmentSubmissionByIdQuery : IRequest<Result<AssignmentSubmissionDTO>>
    {
        public int AssignmentSubmissionId { get; set; }
    }
}
