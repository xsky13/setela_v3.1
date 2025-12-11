using MediatR;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.CreateTopicSeparator
{
    public class CreateTopicSeparatorCommand : IRequest<Result<TopicSeparatorDTO>>
    {
        public int CurrentUserId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
    }
}
