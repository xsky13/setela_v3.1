using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.DTO
{
    public class CreateMultipleResourcesRequestDTO
    {
        public string ParentType { get; set; }
        public int ParentId { get; set; }
        public int CourseId { get; set; }
        public List<IFormFile> Files { get; set; } = [];
    }
}
