using MediatR;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserFeature.Queries.GetProfessorsQuery
{
    public class GetProfessorsQuery : IRequest<Result<List<UserSimpleDTO>>>
    {
    }
}
