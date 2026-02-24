using MediatR;
using SetelaServerV3._1.Application.Features.GradeFeature.DTO;
using SetelaServerV3._1.Shared.Utilities;

namespace SetelaServerV3._1.Application.Features.GradeFeature.Queries.GetGradesForStudentQuery
{
    public class GetGradesForStudentQuery : IRequest<Result<List<GradeDTO>>>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
