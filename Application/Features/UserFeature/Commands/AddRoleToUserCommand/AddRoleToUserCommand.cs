using MediatR;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Commands.AddRoleToUserCommand
{
    public class AddRoleToUserCommand : IRequest<Result<UserDTO>>
    {
        public int UserId { get; set; }
        public UserRoles Role { get; set; }
    }
}
