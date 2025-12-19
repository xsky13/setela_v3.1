using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Application.Features.ModuleFeature.DTO
{
    public class ModuleDTO : IResourceable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public bool Visible { get; set; } = true;
        public DateTime CreationDate { get; set; }
        public int CourseId { get; set; }
        public CourseSimpleDTO Course { get; set; }

        public List<CourseResourceDTO>? Resources { get; set; }
    }
}
