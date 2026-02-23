using MediatR;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.ToggleItemCommand
{
    public class ToggleItemHandler : IRequestHandler<ToggleItemCommand, Result<UserProgressDTO>>
    {
        public async Task<Result<UserProgressDTO>> Handle(ToggleItemCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
