using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Domain.Entities
{
    public class SysUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserImageUrl { get; set; } = "http://localhost:5182/cdn/user_default_image.png"; // FIX
        public List<UserRoles> Roles { get; set; }
        public List<Enrollment> Enrollments { get; set; } = [];
        public List<Course> ProfessorCourses { get; set; } = [];
    }
}
