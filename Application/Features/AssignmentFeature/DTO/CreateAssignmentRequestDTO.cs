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
        [Required(ErrorMessage = "La nota maxima no puede estar vacío.")]
        public int MaxGrade { get; set; }
        [Required(ErrorMessage = "El peso de la nota no puede estar vacío.")]
        public int Weight { get; set; }
        [Required(ErrorMessage = "La fecha de entrega no puede estar vacía."),]
        public DateTime DueDate { get; set; }
    }
}
