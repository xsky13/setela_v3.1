using MediatR;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Commands.UpdateModuleCommand
{
    public class UpdateModuleCommand : IRequest<Result<ModuleSimpleDTO>>
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public string? Title { get; set; }
        public string? TextContent { get; set; }
        public bool? Visible { get; set; }
    }
}
