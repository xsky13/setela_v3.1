using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using System.Threading.Tasks;

namespace SetelaServerV3._1.Shared.Policies
{
    public class Permissions(AppDbContext _db) : IPermissionHandler
    {
        public async Task<bool> CanEditCourse(int userId, Course course)
        {
            var user = await _db.SysUsers.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null || course == null) return false;

            bool isAdmin = user.Roles.Contains(UserRoles.Admin);
            bool isAssignedProfessor = course.Professors.Any(p => p.Id == user.Id);

            return isAdmin || isAssignedProfessor;
        }

        public bool CanEditCourse(SysUser user, Course course)
        {
            if (user == null || course == null) return false;

            bool isAdmin = user.Roles.Contains(UserRoles.Admin);
            bool isAssignedProfessor = course.Professors.Any(p => p.Id == user.Id);

            return isAdmin || isAssignedProfessor;
        }
    }
}
