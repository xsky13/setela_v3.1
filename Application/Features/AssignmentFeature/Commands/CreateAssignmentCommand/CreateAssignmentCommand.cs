using MediatR;
using SetelaServerV3._1.Application.Features.AssignmentFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.Commands.CreateAssignmentCommand
{
    public class CreateAssignmentCommand : IRequest<Result<Assignment>>
    {
        public int UserId { get; set; }
        public CreateAssignmentRequestDTO Assignment { get; set; }
    }
}
