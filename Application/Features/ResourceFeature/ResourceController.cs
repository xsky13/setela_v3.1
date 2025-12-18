using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.CreateResourceCommand;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.DeleteResourceCommand;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.UpdateResourceCommand;
using SetelaServerV3._1.Application.Features.ResourceFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.ResourceFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Resource>> CreateResource([FromBody] CreateResourceRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new CreateResourceCommand
            {
                Url = request.Url,
                LinkText = request.LinkText,
                Type = request.Type,
                ParentType = request.ParentType,
                ParentId = request.ParentId,
                UserId = int.Parse(userId),
                CourseId = request.CourseId
            });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Resource>> UpdateResource([FromBody] UpdateResourceRequestDTO request, int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new UpdateResourceCommand
            {
                ResourceId = id,
                UserId = int.Parse(userId),
                Url = request.Url,
                LinkText = request.LinkText,
                Type = request.Type
            });
            return response.ToActionResult();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteResource(int id)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _mediator.Send(new DeleteResourceCommand
            {
                ResourceId = id,
                UserId = int.Parse(userId),
            });
            return response.ToActionResult();
        }
    }
}
