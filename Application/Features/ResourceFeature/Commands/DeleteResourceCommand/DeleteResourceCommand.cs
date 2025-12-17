using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.DeleteResourceCommand
{
    public class DeleteResourceCommand : IRequest<Result<object>>
    {
        public int ResourceId { get; set; }
        public int UserId { get; set; }
    }
}
