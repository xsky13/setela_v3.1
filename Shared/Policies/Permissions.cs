using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Infrastructure.Data;

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

        public async Task<bool> CanEditCourse(int userId, int courseId)
        {
            return await _db.SysUsers
                .Include(u => u.ProfessorCourses)
                .Where(u => u.Id == userId)
                .AnyAsync(u =>
                    u.Roles.Contains(UserRoles.Admin) ||
                    u.ProfessorCourses.Any(c => c.Id == courseId)
                );
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

        public async Task<bool> CanModifyAssignmentSubmission(int userId, int ownerId, int courseId)
        {
            var currentUser = await _db.SysUsers.FindAsync(userId);
            if (currentUser == null) return false;

            if (currentUser.Roles.Contains(UserRoles.Admin)) return true;

            if (currentUser.Roles.Contains(UserRoles.Professor))
            {
                if (await CanEditCourse(userId, courseId)) return true;
                else return false;
            }

            if (currentUser.Roles.Contains(UserRoles.Student))
            {
                // check the assignment submissions' owner agains the current user id
                if (ownerId == userId) return true;
                return false;
            }

            return false;
        }

        public async Task<bool> CanModifyResource(ResourceParentType parentType, int userId, int courseId, int? ownerId)
        {
            var currentUser = await _db.SysUsers.FindAsync(userId);
            if (currentUser == null) return false;

            if (currentUser.Roles.Contains(UserRoles.Admin)) return true;

            if (currentUser.Roles.Contains(UserRoles.Professor))
            {
                /**
                 * Professor might also be student
                 * Check resource parent type
                 */
                if (parentType != ResourceParentType.AssignmentSubmission || parentType != ResourceParentType.ExamSubmission)
                {
                    var isProfessorOfCourse = await _db.Entry(currentUser)
                        .Collection(u => u.ProfessorCourses)
                        .Query()
                        .AnyAsync(course => course.Id == courseId);

                    // if the resource that is beign created is not a submission check if the professor is in fact professor of said course
                    return isProfessorOfCourse;
                }
                    
                // Professor is making a submission.
                return true;
            }

            if (currentUser.Roles.Contains(UserRoles.Student))
            {
                // check if student is trying to submit an assignment or not
                if (parentType != ResourceParentType.AssignmentSubmission && parentType != ResourceParentType.ExamSubmission)
                {
                    return false;
                } else
                {
                    // user is creating a resource
                    if (ownerId == null) return true;
                    else
                    {
                        // user is modifying resource
                        if (userId != ownerId) return false;
                        return true;
                    }
                }
            }
            return false;

        }

    }
}
