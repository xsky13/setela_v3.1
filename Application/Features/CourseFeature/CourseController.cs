using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.CourseFeature.Commands.CreateCourseCommand;
using SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourseByIdQuery;
using SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourses;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.CourseFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Course[]>> GetCourse()
        {
            var response = await _mediator.Send(new GetCoursesQuery());
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var response = await _mediator.Send(new GetCourseByIdQuery() { CourseId = id });
            return response.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] CreateCourseCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
