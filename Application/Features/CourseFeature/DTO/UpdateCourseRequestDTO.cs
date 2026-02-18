using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.CourseFeature.DTO
{
    public class UpdateCourseRequestDTO
    {
        [Required(ErrorMessage = "El titulo no puede estar vacio")]
        public string Title { get; set; }
        public string Description { get; set; } = "";
        public bool IsActive { get; set; }

    }
}
