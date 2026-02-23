using AutoMapper;
using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Common.DTO;

namespace SetelaServerV3._1.Shared.Common
{
    public class GeneralMappingProfile : Profile
    {
        public GeneralMappingProfile()
        {
            CreateMap<Course, CourseListingDTO>();

            CreateMap<ExamSubmission, ExamSubmissionSimpleDTO>();
            CreateMap<Exam, ExamSimpleDTO>();
            CreateMap<Grade, GradeSimpleDTO>();
            CreateMap<AssignmentSubmission, AssignmentSubmissionSimpleDTO>();
            CreateMap<Assignment, AssignmentSimpleDTO>();
            CreateMap<Module, ModuleSimpleDTO>();
            CreateMap<Course, CourseSimpleDTO>();

            CreateMap<SysUser, UserSimpleDTO>();
            CreateMap<Resource, CourseResourceDTO>();

            CreateMap<UserProgress, UserProgressDTO>();
        }
    }
}
