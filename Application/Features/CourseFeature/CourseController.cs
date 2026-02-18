using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.CourseFeature.Commands.AddProfesorCommand;
using SetelaServerV3._1.Application.Features.CourseFeature.Commands.CreateCourseCommand;
using SetelaServerV3._1.Application.Features.CourseFeature.Commands.DeleteCourseCommand;
using SetelaServerV3._1.Application.Features.CourseFeature.Commands.DisenrollStudentCommand;
using SetelaServerV3._1.Application.Features.CourseFeature.Commands.EnrollStudentCommand;
using SetelaServerV3._1.Application.Features.CourseFeature.Commands.RemoveProfessorCommand;
using SetelaServerV3._1.Application.Features.CourseFeature.Commands.UpdateCourse;
using SetelaServerV3._1.Application.Features.CourseFeature.DTO;
using SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourseByIdQuery;
using SetelaServerV3._1.Application.Features.CourseFeature.Queries.GetCourses;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.CourseFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<CourseListingDTO>>> GetCourse()
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new GetCoursesQuery { UserId = int.Parse(userId) });
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourse(int id)
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

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Course>> UpdateCourse([FromForm] UpdateCourseRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var command = new UpdateCourseCommand
            {
                Id = id,
                UserId = int.Parse(userId),
                Title = request.Title,
                Description = request.Description,
                IsActive = request.IsActive
            };

            var response = await _mediator.Send(command);
            return response.ToActionResult();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteCourse(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var command = new DeleteCourseCommand
            {
                UserId = int.Parse(userId),
                CourseId = id
            };

            var response = await _mediator.Send(command);
            return response.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/add_professor")]
        public async Task<ActionResult<CourseDTO>> AddProfessor([FromBody] UpdateProfessorsRequestDTO request, int id)
        {
            var response = await _mediator.Send(new AddProfesorCommand { CourseId = id, UserId = request.UserId });
            return response.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/remove_professor")]
        public async Task<ActionResult<CourseDTO>> RemoveProfessor([FromBody] UpdateProfessorsRequestDTO request, int id)
        {
            var response = await _mediator.Send(new RemoveProfessorCommand { CourseId = id, UserId = request.UserId });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPost("{id}/enroll")]
        public async Task<ActionResult<CourseDTO>> Enroll([FromBody] EnrollStudentRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new EnrollStudentCommand { CurrentUserId = int.Parse(userId), CourseId = id, UserId = request.UserId });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPost("{id}/disenroll")]
        public async Task<ActionResult<CourseDTO>> Disnroll([FromBody] EnrollStudentRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DisenrollStudentCommand { CurrentUserId = int.Parse(userId), CourseId = id, UserId = request.UserId });
            return response.ToActionResult();
        }
    }
}
