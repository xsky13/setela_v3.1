using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.DTO
{
    public class CreateResourceRequestDTO
    {
        public string? Url { get; set; }
        public string? LinkText { get; set; }
        public string Type { get; set; }
        public string ParentType { get; set; }
        public int ParentId { get; set; }
        public int CourseId { get; set; }
        public IFormFile? File { get; set; }
    }
}
