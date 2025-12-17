using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.UpdateResourceCommand
{
    public class UpdateResourceCommand : IRequest<Result<Resource>>
    {
        public int ResourceId { get; set; }
        public int UserId { get; set; }
        public string? Url { get; set; }
        public string? LinkText { get; set; }
        public string? Type { get; set; }
    }
}
