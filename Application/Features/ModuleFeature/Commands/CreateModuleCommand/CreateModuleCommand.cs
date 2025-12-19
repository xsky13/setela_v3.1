using MediatR;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Commands.CreateModuleCommand
{
    public class CreateModuleCommand : IRequest<Result<ModuleSimpleDTO>>
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
    }
}
