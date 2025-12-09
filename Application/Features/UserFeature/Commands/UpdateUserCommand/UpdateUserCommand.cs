using MediatR;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : IRequest<Result<UserDTO>>
    {
        public int CurrentUserId { get; set; }
        public int UserId { get; set; }
        public List<UserRoles> UserRoles { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
