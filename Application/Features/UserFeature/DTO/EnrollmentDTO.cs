
namespace SetelaServerV3._1.Application.Features.UserFeature.DTO
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool Valid { get; set; } 
        public int? Grade { get; set; }
    }
}
