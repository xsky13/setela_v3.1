using MediatR;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.UpdateTopicSeparator
{
    public class UpdateTopicSeparatorCommand : IRequest<Result<TopicSeparatorDTO>>
    {
        public int Id { get; set; }
        public string NewTitle { get; set; }
    }
}
