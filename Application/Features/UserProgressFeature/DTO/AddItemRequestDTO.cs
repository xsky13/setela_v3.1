using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.DTO
{
    public class AddItemRequestDTO
    {
        public ProgressParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public int CourseId { get; set; }
    }
}
