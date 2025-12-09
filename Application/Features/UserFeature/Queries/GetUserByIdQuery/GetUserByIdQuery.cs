using MediatR;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUserByIdQuery
{
    public class GetUserByIdQuery : IRequest<Result<UserDTO>>
    {
        public int Id { get; set; }
    }
}
