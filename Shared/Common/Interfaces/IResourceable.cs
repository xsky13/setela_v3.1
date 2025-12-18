using SetelaServerV3._1.Shared.Common.DTO;

namespace SetelaServerV3._1.Shared.Common.Interfaces
{
    public interface IResourceable
    {
        int Id { get; set; }
        List<CourseResourceDTO>? Resources { get; set; }
    }
}
