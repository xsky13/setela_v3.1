using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.CreateResourceCommand;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.DeleteResourceCommand;
using SetelaServerV3._1.Application.Features.ResourceFeature.Commands.UpdateResourceCommand;
using SetelaServerV3._1.Application.Features.ResourceFeature.DTO;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Common.Interfaces;
using SetelaServerV3._1.Shared.Utilities;
using System.Security.Claims;

namespace SetelaServerV3._1.Application.Features.ResourceFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController(IMediator _mediator, IFileStorage _storageService) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Resource>> CreateResource([FromForm] CreateResourceRequestDTO request)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if (request.File == null && string.IsNullOrWhiteSpace(request.Url))
            {
                return BadRequest("El recurso debe tener o una url o un archivo.");
            }

            string finalPath = string.Empty;

            if (request.File != null)
            {
                var uploadResult = await _storageService.SaveFile(request.File, int.Parse(userId));
                if (!uploadResult.Success) return BadRequest(uploadResult.Error);
                finalPath = uploadResult.Value;
            } else finalPath = request.Url!;

                var response = await _mediator.Send(new CreateResourceCommand
                {
                    BaseUrl = $"{Request.Scheme}://{Request.Host}",
                    Url = finalPath,
                    LinkText = request.LinkText,
                    Type = request.Type,
                    ParentType = request.ParentType,
                    ParentId = request.ParentId,
                    UserId = int.Parse(userId),
                    CourseId = request.CourseId,
                    Download = request.Download
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
