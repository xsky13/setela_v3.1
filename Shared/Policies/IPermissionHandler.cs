using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Shared.Policies
{
    public interface IPermissionHandler
    {
        bool CanEditCourse(SysUser user, Course course);
    }
}
