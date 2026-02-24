using MediatR;
using SetelaServerV3._1.Application.Features.UserProgressFeature.DTO;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.Commands.ToggleItemCommand
{
    public class ToggleItemCommand : IRequest<Result<UserProgressDTO>>
    {
        public int UserId { get; set; }
        public AddItemRequestDTO Request{ get; set; }
    }
}
