using MediatR;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.UserProgressFeature.Queries.GetProgressQuery
{
    public class GetProgressQuery : IRequest<Result<int>>
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
