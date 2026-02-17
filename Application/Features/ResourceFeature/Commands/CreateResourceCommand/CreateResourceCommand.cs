using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.CreateResourceCommand
{
    public class CreateResourceCommand : IRequest<Result<Resource>>
    {
        public string BaseUrl { get; set; }
        public string Url { get; set; }
        public string? LinkText { get; set; }
        public string Type { get; set; }
        public string ParentType { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Boolean Download { get; set; }
    }
}
