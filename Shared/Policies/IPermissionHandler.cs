using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Shared.Policies
{
    public interface IPermissionHandler
    {
        bool CanEditUser(int currentUserId, int userToChangeId, List<UserRoles> currentUserRoles);
        Task<bool> CanEditCourse(int userId, Course course);
        Task<bool> CanEditCourse(int userId, int courseId);
        bool CanEditCourse(SysUser user, Course course);
        bool CanChangeStudents(SysUser currentUser, int userToChangeId, int courseId);
        Task<bool> CanModifyResource(ResourceParentType parentType, int userId, int courseId);
    }
}
