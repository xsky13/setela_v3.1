using Microsoft.EntityFrameworkCore;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<SysUser> SysUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<TopicSeparator> TopicSeparators { get; set; }
    }
}
