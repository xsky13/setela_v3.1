using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.UpdateAssignmentCommand
{
    public class UpdateAssignmentCommand : IRequest<Result<AssignmentDTO>>
    {
        public int UserId { get; set; }
        public int AssignmentId { get; set; }
        public UpdateAssignmentRequestDTO Assignment { get; set; }
    }
}
