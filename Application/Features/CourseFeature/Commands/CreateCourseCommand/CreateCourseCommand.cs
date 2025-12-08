using MediatR;
using SetelaServerV3._1.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.CreateCourseCommand
{
    public class CreateCourseCommand : IRequest<Course>
    {
        [Required(ErrorMessage = "El titulo no puede estar vacio")]
        public string Title { get; set; }
        public string Description { get; set; } = "";

    }
}
