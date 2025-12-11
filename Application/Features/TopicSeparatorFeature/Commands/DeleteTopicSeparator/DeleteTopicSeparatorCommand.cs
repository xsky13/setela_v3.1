using MediatR;
using SetelaServerV3._1.Application.Features.TopicSeparatorFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.TopicSeparatorFeature.Commands.DeleteTopicSeparator
{
    public class DeleteTopicSeparatorCommand : IRequest<Result<object>>
    {
        public int Id { get; set; }
    }
}
