using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Shared.Policies
{
    public class Permissions : IPermissionHandler
    {
        public bool CanEditCourse(SysUser user, Course course)
        {
            if (user == null || course == null) return false;

            bool isAdmin = user.Roles.Contains(UserRoles.Admin);
            bool isAssignedProfessor = course.Professors.Any(p => p.Id == user.Id);

            return isAdmin || isAssignedProfessor;
        }
    }
}
