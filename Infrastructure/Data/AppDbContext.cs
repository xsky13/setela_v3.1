using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<SysUser> SysUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<TopicSeparator> TopicSeparators { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamSubmission> ExamSubmissions { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }

            modelBuilder.Entity<Resource>()
                .HasOne<Course>()
                .WithMany()
                .HasForeignKey(r => r.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }

       
    }
}
