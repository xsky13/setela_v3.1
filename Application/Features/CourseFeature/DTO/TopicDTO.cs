using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Application.Features.CourseFeature.DTO
{
    public class TopicDTO : IResourceable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<CourseResourceDTO>? Resources { get; set; } = [];

    }
}
