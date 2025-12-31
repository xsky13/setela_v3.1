using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Shared.Common.DTO;

namespace SetelaServerV3._1.Application.Features.UserFeature.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRoles[] Roles { get; set; }
        public List<EnrollmentDTO> Enrollments { get; set; } = [];
        public List<CourseDetailedDTO> ProfessorCourses { get; set; } = [];
    }
}
