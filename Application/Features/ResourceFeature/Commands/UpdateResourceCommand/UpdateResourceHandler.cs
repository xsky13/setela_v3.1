using MediatR;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using SetelaServerV3._1.Shared.Policies;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.ResourceFeature.Commands.UpdateResourceCommand
{
    public class UpdateResourceHandler(AppDbContext _db, IPermissionHandler _userPermissions) : IRequestHandler<UpdateResourceCommand, Result<Resource>>
    {
        public async Task<Result<Resource>> Handle(UpdateResourceCommand command, CancellationToken cancellationToken)
        {
            var resource = await _db.Resources.FindAsync([command.ResourceId], cancellationToken);
            if (resource == null) return Result<Resource>.Fail("El recurso no existe");

            if (!await _userPermissions.CanModifyResource(resource.ParentType, command.UserId, resource.CourseId, resource.OwnerId))
                return Result<Resource>.Fail("No puede modificar este recurso", 403);


            if (command.Type != null)
            {
                if (!Enum.TryParse(command.Type, true, out ResourceType resourceType))
                    return Result<Resource>.Fail("Incorrect resource type");

                resource.ResourceType = resourceType;
            }

            if (command.Url != null) resource.Url = command.Url;
            if (command.LinkText != null) resource.LinkText = command.LinkText;

            return Result<Resource>.Ok(resource);
        }
    }
}
