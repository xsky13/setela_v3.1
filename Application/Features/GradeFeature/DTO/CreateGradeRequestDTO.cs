using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Application.Features.GradeFeature.DTO
{
    public class CreateGradeRequestDTO
    {
        public int Value { get; set; }
        public string ParentType { get; set; }
        public int ParentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
