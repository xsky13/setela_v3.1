using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;
using System.Threading.Tasks;

namespace SetelaServerV3._1.Shared.Policies
{
    public class Permissions(AppDbContext _db) : IPermissionHandler
    {
        public bool CanChangeStudents(SysUser currentUser, int userToChangeId, int courseId)
        {
            if (currentUser.Roles.Contains(UserRoles.Admin)) return true;

            if (currentUser.Roles.Contains(UserRoles.Student))
            {
                if (currentUser.Id == userToChangeId) return true;
                return false;
            } else if (currentUser.Roles.Contains(UserRoles.Professor))
            {
                if (currentUser.ProfessorCourses.Any(c => c.Id == courseId)) return true;
                return false;
            }

            return false;
        }

        public async Task<bool> CanEditCourse(int userId, Course course)
        {
            var user = await _db.SysUsers.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null || course == null) return false;

            bool isAdmin = user.Roles.Contains(UserRoles.Admin);
            bool isAssignedProfessor = course.Professors.Any(p => p.Id == user.Id);

            return isAdmin || isAssignedProfessor;
        }

        public async Task<bool> CanEditCourse(int userId, int courseId)
        {
            var user = await _db.SysUsers.FirstOrDefaultAsync(user => user.Id == userId);
            var course = await _db.Courses.FirstOrDefaultAsync(course => course.Id == courseId);
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

        public bool CanEditUser(int currentUserId, int userToChangeId, List<UserRoles> currentUserRoles)
        {
            if (currentUserRoles.Contains(UserRoles.Admin)) return true;
            else
            {
                if (currentUserId != userToChangeId) return false;
                return true;
            }
        }

        public async Task<bool> CanCreateResource(ResourceParentType parentType, int parentId, int userId)
        {
            var currentUser = await _db.SysUsers.FindAsync(userId);
            if (currentUser == null) return false;

            if (currentUser.Roles.Contains(UserRoles.Student))
            {
                if (parentType != ResourceParentType.AssignmentSubmission || parentType != ResourceParentType.ExamSubmission)
                    return false;
                return true;
            }
            /* TODO */
            /* ADD PERMISSIONS FOR PROFESSOR AND ADMIN, TOO LAZY TOO DO IT NOW*/
            return true;

        }

    }
}
