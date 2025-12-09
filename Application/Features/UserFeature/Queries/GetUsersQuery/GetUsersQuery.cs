using MediatR;
using SetelaServerV3._1.Application.Features.UserFeature.DTO;

namespace SetelaServerV3._1.Application.Features.UserFeature.Queries.GetUsersQuery
{
    public class GetUsersQuery : IRequest<List<UserDTO>>
    {
    }
}
