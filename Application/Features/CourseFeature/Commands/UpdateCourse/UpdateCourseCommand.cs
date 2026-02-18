using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.UpdateCourse
{
    public class UpdateCourseCommand : IRequest<Result<Course>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
