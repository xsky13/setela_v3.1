using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Shared.Policies
{
    public interface IPermissionHandler
    {
        Task<bool> CanEditCourse(int userId, Course course);
        bool CanEditCourse(SysUser user, Course course);
        bool CanChangeStudents(SysUser currentUser, int userToChangeId, int courseId);
    }
}
