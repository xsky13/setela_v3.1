using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;

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
        public List<ProfessorCoursesDTO> ProfessorCourses { get; set; } = [];
    }
}
