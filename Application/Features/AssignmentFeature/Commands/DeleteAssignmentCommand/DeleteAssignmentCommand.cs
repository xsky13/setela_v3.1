using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.DeleteAssignmentCommand
{
    public class DeleteAssignmentCommand : IRequest<Result<object>>
    {
        public int UserId { get; set; }
        public int AssignmentId { get; set; }
    }
}
