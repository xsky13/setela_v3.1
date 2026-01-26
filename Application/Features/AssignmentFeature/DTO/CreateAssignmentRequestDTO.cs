using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.AssignmentFeature.DTO
{
    public class CreateAssignmentRequestDTO
    {
        public int CourseId { get; set; }
        [Required(ErrorMessage = "El titulo no puede estar vacío."), MinLength(1, ErrorMessage = "El titulo no puede estar vacío.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "El contenido no puede estar vacío."), MinLength(1, ErrorMessage = "El contenido no puede estar vacío.")]
        public string TextContent { get; set; }
        public int MaxGrade { get; set; }
        public int Weight { get; set; }
        public DateTime DueDate { get; set; }
    }
}
