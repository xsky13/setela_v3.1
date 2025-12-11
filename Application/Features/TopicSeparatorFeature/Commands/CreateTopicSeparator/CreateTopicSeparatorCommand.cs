using MediatR;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.CreateTopicSeparator
{
    public class CreateTopicSeparatorCommand : IRequest<TopicSeparatorDTO>
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
    }
}
