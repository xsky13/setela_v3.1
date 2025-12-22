using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Queries.GetAssignmentByIdQuery
{
    public class GetAssignmentByIdQuery : IRequest<Result<AssignmentDTO>>
    {
        public int AssignmentId { get; set; }
    }
}
