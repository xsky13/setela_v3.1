using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.Queries.GetModuleByIdQuery
{
    public class GetModuleByIdQuery : IRequest<Result<>>
    {
        public int ModuleId { get; set; }
    }
}
