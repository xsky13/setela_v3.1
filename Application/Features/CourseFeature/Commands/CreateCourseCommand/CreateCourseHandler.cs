using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Infrastructure.Data;

namespace SetelaServerV3._1.Application.Features.CourseFeature.Commands.CreateCourseCommand
{
    public class CreateCourseHandler(AppDbContext _db) : IRequestHandler<CreateCourseCommand, Course>
    {
        public async Task<Course> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
        {
            var newCourse = new Course
            {
                Title = command.Title,
                Description = command.Description
            };
            _db.Courses.Add(newCourse);
            await _db.SaveChangesAsync(cancellationToken);

            return newCourse;
        }
    }
}
