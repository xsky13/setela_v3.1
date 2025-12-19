using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Commands.DeleteModuleCommand
{
    public class DeleteModuleCommand : IRequest<Result<object>>
    {
        public int ModuleId { get; set; }
        public int UserId { get; set; }
    }
}
