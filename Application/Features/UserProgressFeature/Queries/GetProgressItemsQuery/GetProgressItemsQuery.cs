using MediatR;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.Queries.GetProgressItemsQuery
{
    public class GetProgressItemsQuery : IRequest<Result<List<UserProgressDTO>>>
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
